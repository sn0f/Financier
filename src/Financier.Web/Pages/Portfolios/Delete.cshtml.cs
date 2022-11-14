using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Financier.Data;
using Financier.Domain.Portfolios;

namespace Financier.Web.Pages.Portfolios
{
    public class DeleteModel : PageModel
    {
        private readonly FinancierDbContext _context;

        [BindProperty]
        public Portfolio Portfolio { get; set; }

        public DeleteModel(FinancierDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioId = (long)id.Value;
            Portfolio = await _context.Portfolios.FindAsync(portfolioId);

            if (Portfolio != null)
            {
                if (Portfolio.Positions.Any())
                    throw new Exception("Portfolio can not be deleted, because it contains positions.");
                _context.Portfolios.Remove(Portfolio);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
