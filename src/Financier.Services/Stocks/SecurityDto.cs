using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Services.Stocks
{
    public class SecurityDto
    {
        public long Id { get; set; }
        public string ISIN { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SecurityType { get; set; }
    }
}
