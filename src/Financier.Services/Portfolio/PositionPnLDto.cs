using System;
using System.Collections.Generic;
using System.Text;
using Financier.Services.Stocks;

namespace Financier.Services.Portfolio
{
    public class PositionPnLDto
    {
        public SecurityDto Security { get; set; }
        public ProfitAndLossDto PnL { get; set; }
    }
}
