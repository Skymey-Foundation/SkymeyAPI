using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Crypto.Tickers
{
    public class CryptoTickersView
    {
        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }
        [JsonPropertyName("BaseAsset")]
        public string BaseAsset { get; set; }
        [JsonPropertyName("BaseAssetPrecision")]
        public int BaseAssetPrecision { get; set; }
        [JsonPropertyName("QuoteAsset")]
        public string QuoteAsset { get; set; }
        [JsonPropertyName("QuoteAssetPrecision")]
        public int QuoteAssetPrecision { get; set; }
        [JsonPropertyName("Update")]
        public DateTime Update { get; set; }
    }
}
