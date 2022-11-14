using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Services.Portfolio
{
    public class PortfolioPnLDto
    {
        public string Name { get; set; }
        public ProfitAndLossDto PnL { get; set; }
        public List<PositionPnLDto> PositionPnLs { get; set; }
    }
}
