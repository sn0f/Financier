using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financier.Import
{
    public interface IBrokerReportProcessor
    {
        Task LoadAndParseAsync(string fileName);
        void Parse(string text);
        IEnumerable<IImportedTrade> Trades { get; }
        IEnumerable<IImportedMove> Moves { get; }
    }
}
