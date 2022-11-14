using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Financier.Data;
using Financier.Domain.Portfolios;
using Financier.Domain.Stocks;
using Financier.Services.Portfolio;
using Financier.Services.Stocks;
using Financier.Import;
using Financier.Domain.Trades;

namespace Financier.Services.Portfolio
{
    public sealed class PortfolioService : IPortfolioService
    {
        private readonly FinancierDbContext _context;
        private readonly IQuoteService _quotesService;

        public PortfolioService(FinancierDbContext context, IQuoteService quotesService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _quotesService = quotesService ?? throw new ArgumentNullException(nameof(quotesService));
        }


        public async Task<PortfolioPnLDto> GetPnLAsync(long portfolioId, DateTime from, DateTime to)
        {
            var portfolio = await _context.Portfolios.SingleAsync(x => x.Id == portfolioId);
            var positions = portfolio.Positions;

            // TODO take all, not only shares
            positions = positions.Where(x => x.Security.SecurityType == SecurityType.Share).ToList();

            Func<OpenTrade, ProfitAndLoss> getPnL = o =>
            {
                var closedDuring = o.CloseTrades.Where(c => c.DateTime > from && c.DateTime.Date <= to);

                var pnL = new ProfitAndLoss();

                if (o.DateTime < from)
                {
                    var startAmount = o.Amount - o.CloseTrades.Where(c => c.DateTime < from).Sum(c => c.Amount);
                    var startPrice = _quotesService.GetPrice(o.Security, from);

                    pnL.StartVolume = startAmount * startPrice;

                    var closedAmount = closedDuring.Sum(c => c.Amount);
                    var endAmount = startAmount - closedAmount;
                    var endPrice = _quotesService.GetPrice(o.Security, to);

                    pnL.ClosedVolume = closedDuring.Sum(c => c.Amount * c.Price);
                    pnL.NotClosedVolume = endAmount * endPrice;
                }
                else if (o.DateTime > from && o.DateTime < to)
                {
                    pnL.AddedVolume = o.Amount * o.Price;

                    var closedAmount = closedDuring.Sum(c => c.Amount);
                    var endAmount = o.Amount - closedAmount;
                    var endPrice = _quotesService.GetPrice(o.Security, to);

                    pnL.ClosedVolume = closedDuring.Sum(c => c.Amount * c.Price);
                    pnL.NotClosedVolume = endAmount * endPrice;
                }

                return pnL;
            };

            var changes = positions
                .Select(x => new
                {
                    x.Security,
                    PnL = x.OpenedTrades.Select(x => getPnL(x)).Aggregate((x1, x2) => x1 + x2),
                    Position = x,
                }).OrderByDescending(x => x.PnL.Profit).ToList();

            if (!changes.Any())
                return new PortfolioPnLDto
                {
                    Name = portfolio.Name,
                    PnL = default(ProfitAndLossDto),
                    PositionPnLs = new List<PositionPnLDto>(),
                };

            var changesSum = changes?.Select(x => x.PnL).Where(x => x is { }).Aggregate((x1, x2) => x1 + x2);

            var result = new PortfolioPnLDto
            {
                Name = portfolio.Name,
                PnL = changesSum.ToDto(),
                PositionPnLs = changes
                    .Where(x => !(x.PnL.StartVolume == 0.0M && x.PnL.ClosedVolume == 0.0M
                        && x.PnL.AddedVolume == 0.0M && x.PnL.NotClosedVolume == 0.0M))
                    .Select(x => new PositionPnLDto
                    {
                        Security = x.Security.ToDto(),
                        PnL = x.PnL.ToDto(),
                    }
                ).ToList(),
            };

            return result;
        }

        public async Task ImportTradesAsync(long portfolioId, IEnumerable<IImportedTrade> importedTrades)
        {
            var portfolio = await _context.Portfolios.SingleAsync(x => x.Id == portfolioId);
            var positions = portfolio.Positions;

            var trades = importedTrades
                .OrderBy(x => x.Date).ThenBy(x => x.Time)
                .ToList();
            
            foreach(var importedTrade in trades)
            {
                var security = await _context.Securities.SingleOrDefaultAsync(
                    x => x.Code == importedTrade.SecurityCode || x.ISIN == importedTrade.SecurityCode);

                if (security is null)
                    continue;

                //// TODO zaplatka
                //if (security.Code == "EONR")
                //    security = await _context.Securities.SingleOrDefaultAsync(x => x.Code == "UPRO");

                var position = positions.FirstOrDefault(x => x.Security.Code == security.Code);
                if (position is null)
                {
                    position = new Position { Security = security, Portfolio = portfolio, };
                    positions.Add(position);
                }

                var direction = ImportedTradeExtensions.GetDirection(importedTrade);
                var dateTime = importedTrade.Date + importedTrade.Time;

                var trade = new Trade()
                {
                    Security = security,
                    Portfolio = portfolio,
                    Direction = direction,
                    DateTime = dateTime,
                    Amount = Math.Abs(importedTrade.Amount),
                    Price = importedTrade.Price,
                    BrokerCommission = Math.Abs(importedTrade.BrokerCommission),
                    ExchangeCommission = Math.Abs(importedTrade.ExchangeCommission),
                    ExchangeTradeNo = importedTrade.ExchangeTradeNo,
                };
                trade.SumTotalCommission();

                position.AddTrade(trade);
            }

            await _context.SaveChangesAsync();
        }
    }
}
