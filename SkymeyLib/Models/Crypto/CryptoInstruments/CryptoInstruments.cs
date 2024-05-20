using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SkymeyLib.Models.Crypto.CryptoInstruments
{
    public class Datum
    {
        public int id { get; set; }
        public int rank { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public int is_active { get; set; }
        public DateTime first_historical_data { get; set; }
        public DateTime last_historical_data { get; set; }
        public Platform platform { get; set; }
    }

    public class Platform
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public string token_address { get; set; }
    }

    public class PlatformDB
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }
        [JsonPropertyName("Symbol")]
        public string? Symbol { get; set; }
        [JsonPropertyName("Slug")]
        public string? Slug { get; set; }
        [JsonPropertyName("Token_address")]
        public string? Token_address { get; set; }
    }

    public class CryptoInstruments
    {
        public Status status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class CryptoInstrumentsDB
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [JsonPropertyName("_id")]
        public ObjectId? _id { get; set; }
        [JsonPropertyName("Id")]
        public int? Id { get; set; }
        [JsonPropertyName("Rank")]
        public int Rank { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("Slug")]
        public string Slug { get; set; }
        [JsonPropertyName("Is_active")]
        public int Is_active { get; set; }
        [JsonPropertyName("Platform")]
        public PlatformDB? Platform { get; set; }
        [JsonPropertyName("MaxSupply")]
        public string? MaxSupply { get; set; }
        [JsonPropertyName("CurrentSupply")]
        public string? CurrentSupply { get; set; }
        [JsonPropertyName("Mcap")]
        public string? Mcap { get; set; }
        [JsonPropertyName("FDV")]
        public string? FDV { get; set; }
        [JsonPropertyName("Update")]
        public DateTime Update { get; set; }
    }

    public class Status
    {
        public DateTime timestamp { get; set; }
        public int error_code { get; set; }
        public object error_message { get; set; }
        public int elapsed { get; set; }
        public int credit_count { get; set; }
        public object notice { get; set; }
    }
}
