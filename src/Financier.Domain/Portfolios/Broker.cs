using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Financier.Domain.Core;

namespace Financier.Domain.Portfolios
{
    public class Broker : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
