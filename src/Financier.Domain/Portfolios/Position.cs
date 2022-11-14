using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Financier.Domain.Core;
using Financier.Domain.Stocks;
using Financier.Domain.Trades;

namespace Financier.Domain.Portfolios
{
    public class Position : Entity
    {
        [Required]
        public virtual Portfolio Portfolio { get; set; }
        [Required]
        public virtual Security Security { get; set; }
        public decimal Amount { get; protected set; }
        public decimal AveragePrice { get; protected set; }
        [NotMapped]
        public IEnumerable<Trade> Trades => OpenedTrades.Select(x => x.Trade).Union(ClosedTrades.Select(x => x.Trade)).Distinct().ToList();


        public virtual ICollection<OpenTrade> OpenedTrades { get; set; }
        [NotMapped]
        public IEnumerable<CloseTrade> ClosedTrades => OpenedTrades.SelectMany(x => x.CloseTrades).ToList();


        public Position()
        {
            Reset();
        }

        public void Reset()
        {
            OpenedTrades = new List<OpenTrade>();
            Amount = 0;
        }


        public void AddTrade(Trade trade)
        {
            if (Trades.Any(x => x.ExchangeTradeNo == trade.ExchangeTradeNo))
                return;

            if (Amount == 0 || Amount > 0 && trade.Direction == Direction.Buy || Amount < 0 && trade.Direction == Direction.Sell)
                OpenNewPosition(trade, trade.Amount);
            else if (Amount > 0 && trade.Direction == Direction.Sell)
                CloseLongPositions(trade);
            else if (Amount < 0 && trade.Direction == Direction.Buy)
                CloseShortPositions(trade);
        }

        private void OpenNewPosition(Trade trade, decimal amountLeft)
        {
            OpenedTrades.Add(new OpenTrade
            {
                Trade = trade,
                Security = Security,
                Amount = amountLeft,
                PositionType = trade.Direction == Direction.Buy ? PositionType.Long : PositionType.Short,
            });

            Amount += trade.Direction == Direction.Buy ? amountLeft : -amountLeft;
            AveragePrice = OpenedTrades.Where(x => x.AmountLeft > 0).Sum(x => x.AmountLeft * x.Price + x.GetCommission()) / Amount;
        }

        private void CloseLongPositions(Trade trade)
        {
            var amountLeft = trade.Amount;

            var canClosePositions = OpenedTrades.Where(x => x.AmountLeft > 0);

            while (canClosePositions.Any() && amountLeft > 0)
            {
                var openPosition = canClosePositions.First();

                var closingAmount = Math.Min(amountLeft, openPosition.AmountLeft);
                Amount -= closingAmount;

                var closePosition = new CloseTrade
                {
                    OpenTrade = openPosition,
                    Trade = trade,
                    Security = trade.Security,
                    PositionType = PositionType.Long,
                    Amount = closingAmount,
                };

                amountLeft -= closingAmount;
                openPosition.CloseTrades.Add(closePosition);
            }

            if (amountLeft > 0)
            {
                OpenNewPosition(trade, amountLeft);
            }
        }

        // TODO Close short positions
        private void CloseShortPositions(Trade trade)
        {

        }

        public override string ToString()
        {
            return $"{Security?.Code} : {Amount} : {AveragePrice}";
        }
    }
}
