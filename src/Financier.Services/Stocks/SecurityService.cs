using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Financier.Data;
using Financier.Domain.Stocks;
using Financier.Import.Moex;

namespace Financier.Services.Stocks
{
    public class SecurityService : ISecurityService
    {
        private readonly FinancierDbContext _context;

        public SecurityService(FinancierDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public void LoadQuotes(IEnumerable<DataJson> quotes)
        {
            foreach (var quoteJson in quotes)
            {
                var security = _context.Securities.SingleOrDefault(x => x.Code == quoteJson.SECID);
                if (security is null) continue;

                var quote = new Quote
                {
                    Security = security,
                    Period = Period.Day,
                    DateTime = DateTime.ParseExact(quoteJson.TRADEDATE, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Open = (decimal?)quoteJson.OPEN,
                    Close = (decimal?)quoteJson.CLOSE,
                    Low = (decimal?)quoteJson.LOW,
                    High = (decimal?)quoteJson.HIGH,
                };
                _context.Quotes.Add(quote);
            }
        }

        public void LoadSecurities(IEnumerable<SecurityJson> securities, SecurityType securityType)
        {
            foreach (var secJson in securities)
            {
                var security = new Security
                {
                    Code = secJson.SecId,
                    ISIN = secJson.Isin,
                    Name = secJson.ShortName,
                    SecurityType = securityType,
                };
                _context.Securities.Add(security);
            }
        }
    }
}
