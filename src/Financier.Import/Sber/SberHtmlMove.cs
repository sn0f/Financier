using System;

namespace Financier.Import.Sber
{
    public class SberHtmlMove : IImportedMove
    {
        public DateTime Date { get; set; }
        public string SecurityName { get; set; }
        public string SecurityCode { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public string BuyDate { get; set; }
        public string Price { get; set; }
        public string BrokerCommission { get; set; }
        public string ExchangeCommission { get; set; }
        public string OtherCosts { get; set; }
    }
}
