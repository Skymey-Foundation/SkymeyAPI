using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Crypto
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
