using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Stocks;
using Financier.Import.Moex;
using Financier.Services.Stocks;

namespace Financier.Services
{
    public interface IQuoteService
    {
        decimal GetPrice(Security security, DateTime date);
        void LoadDailyQuotes(IEnumerable<DataJson> quotes);
    }
}
