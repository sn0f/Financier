using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Domain.Portfolios
{
    public class ProfitAndLoss
    {
        public decimal StartVolume { get; set; }
        public decimal AddedVolume { get; set; }
        public decimal ClosedVolume { get; set; }
        public decimal NotClosedVolume { get; set; }

        public decimal Profit => ClosedVolume + NotClosedVolume - (StartVolume + AddedVolume);
        public decimal ProfitPercent => Profit / (StartVolume + AddedVolume);

        public override string ToString() => Profit.ToString();

        public static ProfitAndLoss operator +(ProfitAndLoss x, ProfitAndLoss y) => new ProfitAndLoss
        {
            StartVolume = x.StartVolume + y.StartVolume,
            AddedVolume = x.AddedVolume + y.AddedVolume,
            ClosedVolume = x.ClosedVolume + y.ClosedVolume,
            NotClosedVolume = x.NotClosedVolume + y.NotClosedVolume,
        };
    }
}
