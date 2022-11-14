using System;

namespace Financier.Import
{
    public interface IImportedMove
    {
        DateTime Date { get; }
        string SecurityCode { get; }
        string Type { get; }
        public string Reason { get; }
        decimal Amount { get;  }
    }
}
