using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Financier.Data;
using Financier.Domain.Portfolios;
using Financier.Domain.Stocks;
using Financier.Import.Moex;
using Financier.Services;

namespace Financier.Web
{
    public class FinancierDbContextSeed
    {
        public void Seed(FinancierDbContext context, ISecurityService securityService, IQuoteService quoteService)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (securityService is null)
                throw new ArgumentNullException(nameof(securityService));
            if (quoteService is null)
                throw new ArgumentNullException(nameof(quoteService));

            if (context.Brokers.Any())
                return;

            context.Brokers.AddRange(GetBrokers());

            SeedSecurites(securityService);
            context.SaveChanges();

            SeedQuotes(quoteService);
            context.SaveChanges();
        }


        private IEnumerable<Broker> GetBrokers()
        {
            return new List<Broker>()
            {
                new Broker() { Name = "Sber" },
            };
        }

        private void SeedSecurites(ISecurityService securityService)
        {
            var sharesJsonString = File.ReadAllText(@"..\..\data\securities\shares.json");
            var sharesJson = JsonSerializer.Deserialize<List<SecurityJson>>(sharesJsonString);
            securityService.LoadSecurities(sharesJson, SecurityType.Share);

            var ofzJsonString = File.ReadAllText(@"..\..\data\securities\ofz.json");
            var ofzJson = JsonSerializer.Deserialize<List<SecurityJson>>(ofzJsonString);
            securityService.LoadSecurities(ofzJson, SecurityType.Bond);
        }

        private void SeedQuotes(IQuoteService quoteService)
        {
            var dateStart = new DateTime(2013, 3, 25);
            var dateEnd = DateTime.Today.AddDays(1);

            for (var date = dateStart; date < dateEnd; date = date.AddDays(1))
            {
                var fileName = $"TQBR_{date.ToString("yyyy-MM-dd")}.json";
                var filePath = @"..\..\data\quotes\" + fileName;

                if (File.Exists(filePath))
                {
                    var quotesJsonString = File.ReadAllText(filePath);
                    var quotesJson = JsonSerializer.Deserialize<List<DataJson>>(quotesJsonString);

                    quoteService.LoadDailyQuotes(quotesJson);
                }
            }
        }
    }
}
