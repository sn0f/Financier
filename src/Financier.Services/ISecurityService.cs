using System;
using System.Collections.Generic;
using System.Text;
using Financier.Domain.Stocks;
using Financier.Import.Moex;

namespace Financier.Services
{
    public interface ISecurityService
    {
        void LoadSecurities(IEnumerable<SecurityJson> securities, SecurityType securityType);
    }
}
