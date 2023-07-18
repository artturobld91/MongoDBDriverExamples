using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDriverExamples.Models
{
    [BsonIgnoreExtraElements]
    internal sealed class Post
    {
        [BsonId] // Specifies a field that must be unique
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }
    }
}
