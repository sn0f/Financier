using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Financier.Data;
using Financier.Domain.Stocks;
using Financier.Import.Moex;

namespace Financier.Services.Stocks
{
    public class QuoteService : IQuoteService
    {
        private readonly FinancierDbContext _context;

        public QuoteService(FinancierDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public decimal GetPrice(Security security, DateTime targetDate)
        {
            var quotes = _context.Quotes.Where(x => x.Security == security);
            var dates = quotes.Where(x => x.Close.HasValue).Select(x => x.DateTime);
            var nearestDate = dates.Where(date => date <= targetDate).Max();
            var quote = quotes.First(x => x.DateTime == nearestDate);
            return quote.Close.Value;
        }

        public void LoadDailyQuotes(IEnumerable<DataJson> quotes)
        {
            foreach (var quoteJson in quotes)
            {
                var security = _context.Securities.SingleOrDefault(x => x.Code == quoteJson.SECID);
                if (security is null) 
                    continue;

                var quote = new Quote
                {
                    Security = security,
                    Period = Period.Day,
                    DateTime = DateTime.ParseExact(quoteJson.TRADEDATE, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Open = (decimal?)quoteJson.OPEN,
                    Close = (decimal?)quoteJson.CLOSE,
                    Low = (decimal?)quoteJson.LOW,
                    High = (decimal?)quoteJson.HIGH,
                };
                _context.Quotes.Add(quote);
            }
        }
    }
}
