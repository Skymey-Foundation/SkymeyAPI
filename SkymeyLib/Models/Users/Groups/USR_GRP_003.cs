using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace SkymeyLib.Models.Users.Groups
{
    public class USR_GRP_003
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [JsonPropertyName("_id")]
        [BsonElement("_id")]
        public ObjectId? _id { get; set; }

        [JsonPropertyName("Email")]
        [BsonElement("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("GRP_002_ID")]
        [BsonElement("GRP_002_ID")]
        public string? GRP_002_ID { get; set; }
        
        public USR_GRP_003()
        {
            _id = ObjectId.GenerateNewId();
        }
    }
}
