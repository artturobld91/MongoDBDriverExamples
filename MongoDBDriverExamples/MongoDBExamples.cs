using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDriverExamples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDriverExamples
{
    public class MongoDBExamples
    {
        /// <summary>
        /// Notes:
        /// 1. If using C# classes to define the documents, use LINQ.
        /// 2. If using BsonDocuments to represent your data, use the Builders class.
        /// </summary>
        
        MongoClient _client;
        public MongoDBExamples()
        {
            _client = MongoDBConnection.GetConnection();
        }

        public void ListDatabases()
        {
            _client.ListDatabases().ToList().ForEach(database => { Console.WriteLine($"{database}"); });
        }

        public void InsertDocument()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var newPost = new Post
            {
                Name = "test post",
                Age = 1
            };

            blogsCollection.InsertOne(newPost);
        }

        public async Task InsertAsyncDocument()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var newPost = new Post
            {
                Name = "test async post",
                Age = 3
            };

            await blogsCollection.InsertOneAsync(newPost);
        }

        /// <summary>
        /// InsertMany() Takes an IEnumerable of the type that matches the collection type.
        /// This does one trip to mongo.
        /// </summary>
        public async Task InsertManyAsyncDocument()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var newPost = new Post
            {
                Name = "insert many post",
                Age = 5
            };

            var newPostB = new Post
            {
                Name = "insert many post 2",
                Age = 6
            };

            await blogsCollection.InsertManyAsync(new List<Post> { newPost, newPostB });
        }

        /// <summary>
        /// Find() returns an IEnumerable
        /// We can use LINQ to order, limit, skip or do other operations to the results.
        /// </summary>
        /// <returns></returns>
        public async Task FindAsyncDocument()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var posts = await blogsCollection.FindAsync(post => post.Name == "insert many post 2");
            var post = posts.FirstOrDefault();

            Console.WriteLine($"Post found: {post.Name}");

            // Other method to filter
            //var filter = Builders<BsonDocument>.Filter.Eq("Name", "insert many post 2");
            //var document = blogsCollection.Find(filter).FirstOrDefault();
        }

        public void FindListDocument()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var posts = blogsCollection.Find(_ => true).ToList();
            posts.ForEach(post => Console.WriteLine(post.ToString()));
        }

        /// <summary>
        /// UpdateResult
        /// 
        /// IsAcknowledged -> Was the update successful.
        /// MatchedCount -> How many documents were found.
        /// ModifiedCount -> How many documents were updated.
        /// </summary>
        public void UpdateDocument()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var filter = Builders<Post>
                .Filter
                .Eq(post => post.Name, "test post");

            var update = Builders<Post>
                .Update
                .Set(post => post.Name, "test post " + DateTime.Now.ToString());

            //var post = new Post { Name = "test post " + DateTime.Now.ToString() };
            var result = blogsCollection.UpdateOne(filter, update);

            Console.WriteLine($"Acknowledged: {result.IsAcknowledged} - Matched: {result.MatchedCount} - Modified: {result.ModifiedCount}");
        }

        /// <summary>
        /// DeleteOne()
        /// Delete a single document from a collection
        /// DeleteResult
        /// IsAcknowledged -> Was the delete successful.
        /// DeletedCount -> How many documents were found by the filter and deleted.
        /// </summary>
        public void DeleteDocument()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var result = blogsCollection.DeleteOne(post => post.Name == "test post");
            Console.WriteLine($"Acknowledged: {result.IsAcknowledged} - Deleted: {result.DeletedCount}");
        }

        public void Transaction()
        {
            using (var session = _client.StartSession())
            {
                //session.StartTransaction();
                //session.AbortTransaction();
                //session.CommitTransaction();
                //session.WithTransaction((s, ct) =>
                //{

                //});
            }
        }

        /// <summary>
        /// Aggregation framework is used to build multi staged queries.
        /// Think of queries as many stages.
        /// Stage1 -> Stage2 -> Stage3 -> ... StageN
        /// An aggregation pipeline is composed of stages and expression operators.
        /// Stages types:
        /// Finding
        /// Sorting
        /// Grouping
        /// Projecting
        /// </summary>
        public void AggregationMatch()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var matchStage = Builders<Post>
                .Filter
                .Where(post => post.Name.Contains("test"));

            var aggregate = blogsCollection.Aggregate().Match(matchStage);

            var results = aggregate.ToList();

            results.ForEach(res => Console.WriteLine(res.Name));
        }


        public void AggregationGroup()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var matchStage = Builders<Post>
                .Filter
                .Where(post => post.Name.Contains("test"));

            var aggregate = blogsCollection.Aggregate()
                .Match(matchStage)
                .Group(
                    a => a.Type,
                    r => new
                    {
                        postType = r.Key,
                        total = r.Sum(a => 1)
                    }
                );

            var results = aggregate.ToList();

            results.ForEach(res => Console.WriteLine($"{res.postType} - {res.total}"));
        }

        public void AggregationSort()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var matchStage = Builders<Post>
                .Filter
                .Where(post => post.Name.Contains("test"));

            var aggregate = blogsCollection.Aggregate()
                .Match(matchStage)
                .SortByDescending(post => post.Name);

            var results = aggregate.ToList();

            results.ForEach(res => Console.WriteLine($"{res.Name}"));
        }

        public void AggregationProjection()
        {
            var database = _client.GetDatabase("blog");
            var blogsCollection = database.GetCollection<Post>("posts");

            var matchStage = Builders<Post>
                .Filter
                .Where(post => post.Name.Contains("test"));

            var projectStage = Builders<Post>
                .Projection.Expression(post =>
                new
                {
                    Type = post.Type,
                    Age = post.Age
                });

            var aggregate = blogsCollection.Aggregate()
                .Match(matchStage)
                .SortByDescending(post => post.Name)
                .Project(projectStage);

            var results = aggregate.ToList();

            results.ForEach(res => Console.WriteLine($"{res.ToString()}"));
        }
    }
}
