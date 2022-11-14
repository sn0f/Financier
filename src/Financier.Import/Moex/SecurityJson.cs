using System.Text.Json.Serialization;

namespace Financier.Import.Moex
{
    public class SecurityJson
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("secid")]
        public string SecId { get; set; }

        [JsonPropertyName("shortname")]
        public string ShortName { get; set; }

        [JsonPropertyName("regnumber")]
        public string RegNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isin")]
        public string Isin { get; set; }

        [JsonPropertyName("is_traded")]
        public int IsTraded { get; set; }

        [JsonPropertyName("emitent_id")]
        public int? EmitentId { get; set; }

        [JsonPropertyName("emitent_title")]
        public string EmitentTitle { get; set; }

        [JsonPropertyName("emitent_inn")]
        public string EmitentInn { get; set; }

        [JsonPropertyName("emitent_okpo")]
        public string EmitentOkpo { get; set; }

        [JsonPropertyName("gosreg")]
        public string Gosreg { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("primary_boardid")]
        public string PrimaryBoardId { get; set; }

        [JsonPropertyName("marketprice_boardid")]
        public string MarketPriceBoardId { get; set; }

        public override string ToString() => ShortName;
    }
}
