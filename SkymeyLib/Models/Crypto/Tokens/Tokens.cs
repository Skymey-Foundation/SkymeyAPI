using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SkymeyLib.Models.Crypto.Tokens
{
    public class Tokens
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [JsonPropertyName("_id")]
        public ObjectId? _id { get; set; }
        [JsonPropertyName("InstrumentId")]
        public int? InstrumentId { get; set; }
        [JsonPropertyName("Blockchain")]
        public string Blockchain { get; set; }
        [JsonPropertyName("Contract")]
        public string Contract {  get; set; }
        [JsonPropertyName("Update")]
        public DateTime Update {  get; set; }
    }
}
