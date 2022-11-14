using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Financier.Services.Portfolio;
using Financier.Services;

namespace Financier.Web.Pages.Portfolios
{
    public class DetailsModel : PageModel
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioPnLDto PortfolioPnL { get; private set; }
        public long PortfolioId { get; private set; }


        public DetailsModel(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PortfolioId = id.Value;
            var dateFrom = new DateTime(2000, 1, 1);
            var dateTo = DateTime.Today;

            PortfolioPnL = await _portfolioService.GetPnLAsync(PortfolioId, dateFrom, dateTo);

            if (PortfolioPnL == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
