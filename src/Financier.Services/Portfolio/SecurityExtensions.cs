using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Portfolios;

namespace Financier.Services.Portfolio
{
    public static class ProfitAndLossExtensions
    {
        public static ProfitAndLossDto ToDto(this ProfitAndLoss pnL)
        {
            return new ProfitAndLossDto
            {
                StartVolume = pnL.StartVolume,
                AddedVolume = pnL.AddedVolume,
                ClosedVolume = pnL.ClosedVolume,
                NotClosedVolume = pnL.NotClosedVolume,
            };
        }
    }
}
