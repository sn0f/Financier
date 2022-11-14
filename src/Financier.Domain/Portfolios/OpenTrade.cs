using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Financier.Domain.Core;
using Financier.Domain.Stocks;
using Financier.Domain.Trades;

namespace Financier.Domain.Portfolios
{
    public class OpenTrade : Entity
    {
        public virtual Position Position { get; set; }
        public virtual Security Security { get; set; }
        public virtual PositionType PositionType { get; set; }

        public virtual Trade Trade { get; set; }
        public decimal Amount { get; set; }

        public DateTime DateTime => Trade.DateTime;
        public decimal Price => Trade.Price;
        public decimal AmountLeft => Amount - CloseTrades.Sum(x => x.Amount);

        public virtual ICollection<CloseTrade> CloseTrades { get; set; }

        public decimal GetCommission()
        {
            return Trade.TotalCommission * (Amount / Trade.Amount);
        }

        public OpenTrade()
        {
            CloseTrades = new List<CloseTrade>();
        }
    }
}
