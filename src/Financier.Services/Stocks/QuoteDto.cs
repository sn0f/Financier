using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Services.Stocks
{
    public class QuoteDto
    {
        public SecurityDto Security { get; set; }
        public DateTime DateTime { get; set; }
        public string Period { get; set; }

        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? Low { get; set; }
        public decimal? High { get; set; }
    }
}
