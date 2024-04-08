using System.Text.Json.Serialization;

namespace SkymeyCryptoService.Models.Crypto
{
    public class CryptoActualPricesView
    {
        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }
        [JsonPropertyName("Price")]
        public double Price { get; set; }
        [JsonPropertyName("Update")]
        public DateTime Update { get; set; }
    }
}
