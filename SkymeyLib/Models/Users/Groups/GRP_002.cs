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
    public class GRP_002
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [JsonPropertyName("_id")]
        [BsonElement("_id")]
        public ObjectId? _id { get; set; }
        [JsonPropertyName("Id")]
        [BsonElement("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Title")]
        [BsonElement("Title")]
        public string Title { get; set; }

        public GRP_002()
        {
            _id = ObjectId.GenerateNewId();
        }
    }
}
