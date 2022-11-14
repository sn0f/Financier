using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Domain.Portfolios
{
    public class Payout
    {
        public Position Position { get; set; }
        public PayoutType PayoutType { get; set; }


        public decimal StockAmount { get; set; }
        public decimal Value { get; set; }
    }
}
