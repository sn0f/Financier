using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Core;
using Financier.Domain.Stocks;
using Financier.Domain.Trades;

namespace Financier.Domain.Portfolios
{
    public class CloseTrade : Entity
    {
        public virtual Position Position { get; set; }
        public virtual Security Security { get; set; }
        public PositionType PositionType { get; set; }

        public virtual OpenTrade OpenTrade { get; set; }
        public virtual Trade Trade { get; set; }

        public decimal Amount { get; set; }
        public DateTime DateTime => Trade.DateTime;
        public decimal Price => Trade.Price;


        public decimal GetPnL()
        {
            var pnl = PositionType switch
            {
                PositionType.Long => (Price - OpenTrade.Price) * Amount,
                PositionType.Short => (OpenTrade.Price - Price) * Amount,
                _ => throw new NotSupportedException($"{PositionType}"),
            };
            return pnl - GetCommission();
        }

        public decimal GetCommission()
        {
            return OpenTrade.Trade.TotalCommission * (Amount / OpenTrade.Trade.Amount) + Trade.TotalCommission * (Amount / Trade.Amount);
        }

        public CloseTrade()
        {

        }
    }
}
