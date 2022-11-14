using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Stocks;

namespace Financier.Domain.Portfolios
{
    public class PayoutReason
    {
        public Security Security { get; set; }
        public DateTime DateTime { get; set; }
        public PayoutType PayoutType { get; set; } 
        public decimal PayoutAmount { get; set; }

        // TODO Для дивиденда указывать квартал/год или другой период, для облигаций период
        public string Reason { get; set; }
    }
}
