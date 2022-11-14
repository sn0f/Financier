using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Financier.Data;
using Financier.Domain.Portfolios;

namespace Financier.Web.Pages.Portfolios
{
    public class IndexModel : PageModel
    {
        private readonly FinancierDbContext _context;

        public IndexModel(FinancierDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IList<Portfolio> Portfolios { get; set; }

        public async Task OnGetAsync()
        {
            Portfolios = await _context.Portfolios.ToListAsync();
        }
    }
}
