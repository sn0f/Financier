using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Financier.Domain.Core;

namespace Financier.Domain.Stocks
{
    public class Security : Entity
    {
        public string ISIN { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public SecurityType SecurityType { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }
    }
}
