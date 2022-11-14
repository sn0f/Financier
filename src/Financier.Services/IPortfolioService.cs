using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Financier.Import;
using Financier.Services.Portfolio;

namespace Financier.Services
{
    public interface IPortfolioService
    {
        Task<PortfolioPnLDto> GetPnLAsync(long portfolioId, DateTime from, DateTime to);
        Task ImportTradesAsync(long portfolioId, IEnumerable<IImportedTrade> importedTrades);
    }
}
