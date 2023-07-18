using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDriverExamples.Models
{
    // C# Classes for Mongo
    // Provide type safety
    // Make working with MongoDB data much easier
    internal sealed class Account
    {
        [BsonId] // Specifies a field that must be unique
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("account_id")]
        public string AccountId { get; set; } = string.Empty;

        [BsonElement("account_holder")]
        public string AccountHolder { get; set; } = string.Empty;

        [BsonElement("account_type")]
        public string AccountType { get; set; } = string.Empty;

        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        [BsonElement("balance")]
        public decimal Balance { get; set; }

        [BsonElement("transfers_complete")]
        public string[] TransfersCompleted = Array.Empty<string>();
    }

    // BSON Document object
    // Useful when working with schemaless data

    //var document = new BsonDocument
    //{
    //    { "account_id", "MDB829001337" },
    //    { "account_holder", "Linus Torvalds"},
    //    { "account_type", "checking"},
    //    { "balance", 50352434}
    //};
}
