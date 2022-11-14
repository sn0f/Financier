using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Financier.Data;
using Financier.Domain.Portfolios;

namespace Financier.Web.Pages.Portfolios
{
    public class CreateModel : PageModel
    {
        private readonly FinancierDbContext _context;

        [BindProperty]
        public Portfolio Portfolio { get; set; }
        public SelectList Brokers { get; set; }
        public long BrokerId { get; set; }

        public CreateModel(FinancierDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult OnGet()
        {
            var brokers = _context.Brokers.ToList();
            var broker = brokers.First();
            Portfolio = new Portfolio { Name = string.Empty, Broker = broker };
            Brokers = new SelectList(brokers, nameof(Broker.Id), nameof(Broker.Name), broker);
            BrokerId = brokers.First().Id;
            ViewData["Brokers"] = Brokers;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Portfolios.Add(Portfolio);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
