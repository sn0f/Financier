using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Stocks;

namespace Financier.Services.Stocks
{
    public static class SecurityExtensions
    {
        public static SecurityDto ToDto(this Security security)
        {
            return new SecurityDto
            {
                Id = security.Id,
                Code = security.Code,
                ISIN = security.ISIN,
                Name = security.Name,
                SecurityType = security.SecurityType.ToString()
            };
        }
    }
}
