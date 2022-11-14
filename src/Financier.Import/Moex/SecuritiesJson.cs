using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Financier.Import.Moex
{
    public class SecuritiesJson
    {

        [JsonPropertyName("charsetinfo")]
        public CharsetinfoJson CharsetInfo { get; set; }

        [JsonPropertyName("securities")]
        public IList<SecurityJson> Securities { get; set; }
    }

}
