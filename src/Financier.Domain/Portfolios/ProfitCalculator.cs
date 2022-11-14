using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Financier.Domain.Stocks;

namespace Financier.Domain.Portfolios
{
    public class ProfitCalculator
    {
        public IEnumerable<Quote> Quotes { get; set; }

        public Position Position { get; set; }

        public ProfitCalculator()
        {

        }
    }
}
