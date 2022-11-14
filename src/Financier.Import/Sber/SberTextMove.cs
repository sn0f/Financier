using System;

namespace Financier.Import.Sber
{
    public class SberTextMove : IImportedMove
    {
        public DateTime Date { get; set; }
        public string SecurityName { get; set; }
        public string SecurityCode { get; set; }
        public string OperationCode { get; set; }
        public string Type { get; set; }
        public string BankOperationNo { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public string BuyDate { get; set; }
        public decimal Price { get; set; }
        public decimal BrokerCommission { get; set; }
        public decimal ExchangeCommission { get; set; }
        public string OtherCosts { get; set; }
    }
}
