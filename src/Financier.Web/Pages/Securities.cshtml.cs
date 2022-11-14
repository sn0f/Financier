using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Financier.Data;
using Financier.Domain.Stocks;

namespace Financier.Web
{
    public class SecuritiesModel : PageModel
    {
        private readonly FinancierDbContext _context;

        public SecuritiesModel(FinancierDbContext context)
        {
            _context = context;
        }

        public IList<Security> Securities { get; set; }

        public async Task OnGetAsync()
        {
            Securities = await _context.Securities.ToListAsync();
        }
    }
}
