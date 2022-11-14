using System;

namespace Financier.Import.Sber
{
    public class SberHtmlTrade : IImportedTrade
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime ContractDate { get; set; }
        public string SecurityName { get; set; }
        public string SecurityCode { get; set; }
        public string Currency { get; set; }
        public string Direction { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal BrokerCommission { get; set; }
        public decimal ExchangeCommission { get; set; }
        public long ExchangeTradeNo { get; set; }
        public string Comment { get; set; }
        public string TradeStatus { get; set; }
    }
}
