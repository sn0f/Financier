using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Financier.Domain.Core;

namespace Financier.Domain.Portfolios
{
    public class Portfolio : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long BrokerId { get; set; }
        public virtual Broker Broker { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
