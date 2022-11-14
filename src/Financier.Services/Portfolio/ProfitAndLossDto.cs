using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Services.Portfolio
{
    public class ProfitAndLossDto
    {
        public decimal StartVolume { get; set; }
        public decimal AddedVolume { get; set; }
        public decimal ClosedVolume { get; set; }
        public decimal NotClosedVolume { get; set; }

        public decimal Profit => ClosedVolume + NotClosedVolume - (StartVolume + AddedVolume);
        public decimal ProfitPercent => Profit / (StartVolume + AddedVolume);
    }
}
