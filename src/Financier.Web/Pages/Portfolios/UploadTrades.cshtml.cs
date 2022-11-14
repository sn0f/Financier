using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Financier.Data;
using Financier.Import.Sber;
using Financier.Import;
using Financier.Services;
using Financier.Domain.Portfolios;

namespace Financier.Web.Pages
{
    public class UploadTradesModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FinancierDbContext _context;
        private readonly IPortfolioService _portfolioService;
        private readonly Encoding _encoding;

        [BindProperty]
        public Portfolio Portfolio { get; set; }

        public UploadTradesModel(IWebHostEnvironment webHostEnvironment, FinancierDbContext context, IPortfolioService portfolioService)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _encoding = Encoding.GetEncoding("windows-1251");
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Portfolio = await _context.Portfolios.FirstOrDefaultAsync(m => m.Id == id);

            if (Portfolio == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id, IFormFile[] files)
        {
            if (files != null && files.Length > 0)
            {
                var trades = new List<IImportedTrade>();

                foreach (IFormFile file in files)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var brokerReportProcessor = SberBrokerReportProcessorFactory.CreateFromFileExtension(fileExtension);

                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        ms.Position = 0;

                        using (var sr = new StreamReader(ms, _encoding))
                        {
                            var text = sr.ReadToEnd();

                            brokerReportProcessor.Parse(text);
                            if (brokerReportProcessor.Trades.Any())
                                trades.AddRange(brokerReportProcessor.Trades);
                        }
                    }
                }

                var uniqueTrades = trades.GroupBy(x => x.ExchangeTradeNo).Select(x => x.First()).ToList();
                await _portfolioService.ImportTradesAsync(id.Value, uniqueTrades);
            }

            return RedirectToPage("./Details", new { id });
        }
    }
}
