using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SkymeyLib.Models.Users.Table
{
    public class USR_001
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [JsonPropertyName("_id")]
        [BsonElement("_id")]
        public ObjectId? _id { get; set; }
        [JsonPropertyName("Email")]
        [BsonElement("Email")]
        public string Email {  get; set; }
        [JsonPropertyName("Password")]
        [BsonElement("Password")]
        public string Password { get; set; }
        [JsonPropertyName("RefreshToken")]
        [BsonElement("RefreshToken")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("RefreshTokenExpiryTime")]
        [BsonElement("RefreshTokenExpiryTime")]
        public DateTime RefreshTokenExpiryTime { get; set; }
        public USR_001()
        {
            _id = ObjectId.GenerateNewId();
        }
    }

}
