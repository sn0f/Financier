using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Domain.Portfolios
{
    public class FinancialResult
    {
        public Position Position { get; set; }
        public DateTime DateTime { get; set; }

        public decimal OpenPositionsPnL { get; set; }
        public decimal ClosedPositionsPnL { get; set; }
        public decimal Payouts { get; set; }
        
        public decimal BrokerCommission { get; set; }
        public decimal ExchangeCommission { get; set; }
        public decimal TotalCommission { get; set; }
    }
}
