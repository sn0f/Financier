using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Financier.Domain.Core;
using Financier.Domain.Portfolios;
using Financier.Domain.Stocks;

namespace Financier.Domain.Trades
{
    public class Trade : Entity
    {
        [Required]
        public virtual Portfolio Portfolio { get; set; }
        [Required]
        public virtual Security Security { get; set; }
        public Direction Direction { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal ExchangeCommission { get; set; }
        public decimal BrokerCommission { get; set; }
        public decimal TotalCommission { get; set; }
        public long ExchangeTradeNo { get; set; }

        public void SumTotalCommission() => TotalCommission = BrokerCommission + ExchangeCommission;
    }
}
