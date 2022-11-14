using System;
using Financier.Domain.Trades;
using Financier.Import.Sber;

namespace Financier.Import
{
    public interface IImportedTrade
    {
        DateTime Date { get; }
        TimeSpan Time { get; }
        public string SecurityCode { get; }
        public string Direction { get; }
        public decimal Amount { get; }
        public decimal Price { get; }
        public decimal Volume { get; }
        public decimal AccruedInterest { get; }
        public decimal BrokerCommission { get; }
        public decimal ExchangeCommission { get; }
        public long ExchangeTradeNo { get; }
    }

    public static class ImportedTradeExtensions
    {
        public static Direction GetDirection(this IImportedTrade importedTrade)
        {
            if (importedTrade is SberHtmlTrade)
                return importedTrade.Direction == "Покупка" ? Direction.Buy : Direction.Sell;
            if (importedTrade is SberTextTrade)
                return importedTrade.Direction == "B" ? Direction.Buy : Direction.Sell;

            throw new NotSupportedException($"Unprocessable type {importedTrade.GetType()}");
        }
    }
}
