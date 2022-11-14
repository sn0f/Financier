using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Stocks;
using Financier.Domain.Trades;

namespace Financier.Domain.Portfolios
{
    public class OpenedPosition
    {
        public Portfolio Portfolio { get; set; }
        public Security Security { get; set; }
        public PositionType PositionType { get; set; }
        public Trade Trade { get; set; }
        public decimal AmountLeft { get; set; }

        public decimal GetCommission()
        {
            return Trade.TotalCommission * (AmountLeft / Trade.Amount);
        }
    }
}
