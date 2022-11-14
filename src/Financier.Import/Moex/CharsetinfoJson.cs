using System.Text.Json.Serialization;

namespace Financier.Import.Moex
{
    public class CharsetinfoJson
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
