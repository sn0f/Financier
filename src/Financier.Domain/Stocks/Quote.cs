using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financier.Domain.Stocks
{
    public class Quote
    {
        [Required]
        public Period Period { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public long SecurityId { get; set; }

        public virtual Security Security { get; set; }

        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? Low { get; set; }
        public decimal? High { get; set; }
    }
}
