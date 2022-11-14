using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Stocks;
using Financier.Domain.Trades;

namespace Financier.Domain.Portfolios
{
    public class ClosedPosition
    {
        public Portfolio Portfolio { get; set; }
        public Security Security { get; set; }
        public decimal Amount { get; set; }
        public PositionType PositionType { get; set; }

        public DateTime OpenDateTime { get; set; }
        public decimal OpenPrice { get; set; }
        public Trade OpenTrade { get; set; }

        public DateTime CloseDateTime { get; set; }
        public decimal ClosePrice { get; set; }
        public Trade CloseTrade { get; set; }

        //public IEnumerable<Payout> Payouts { get; set; }

        public decimal GetPnL()
        {
            var pnl = PositionType switch
            {
                PositionType.Long => (ClosePrice - OpenPrice) * Amount,
                PositionType.Short => (OpenPrice - ClosePrice) * Amount,
                _ => throw new NotSupportedException($"{PositionType}"),
            };
            return pnl - GetCommission();
        }

        public decimal GetCommission() {
            return OpenTrade.TotalCommission * (Amount / OpenTrade.Amount) + CloseTrade.TotalCommission * (Amount / CloseTrade.Amount);
        }
    }
}
