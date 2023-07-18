using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;


namespace MongoDBDriverExamples
{
    /// <summary>
    /// Notes: An application should use a single MongoClient instance to be reused across all database requests.
    /// Creating MongoClients is resource intensive.
    /// Creating a new MongoClient for each request will affect the application's performance.
    /// Applying singleton pattern to get a single instance of the MongoClient Class
    /// </summary>
    public sealed class MongoDBConnection
    {
        // Connection String Example
        // public readonly string connectionUri = "mongodb+srv://{user}:{password}@{cluster}/?retryWrites=true&w=majority";

        private static MongoClient? _connection;
        private MongoDBConnection() {
           
        }

        public static MongoClient GetConnection() {
            if (_connection == null)
            {
                var secretAppsettingReader = new SecretsSettingsReader();
                var secretValues = secretAppsettingReader.ReadSection<MongoDBSettings>("MongoDBConfig");
                Console.WriteLine($"Cluster: {secretValues.Cluster}");
                Console.WriteLine($"User: {secretValues.User}");
                Console.WriteLine($"Password: {secretValues.Password}");

                string connectionUri = $"mongodb+srv://{secretValues.User}:{secretValues.Password}@{secretValues.Cluster}/?retryWrites=true&w=majority";

                var settings = MongoClientSettings.FromConnectionString(connectionUri);

                // Set the ServerApi field of the settings object to Stable API version 1
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);

                // Create a new client and connect to the server
                _connection = new MongoClient(settings);

                // Send a ping to confirm a successful connection
                try
                {
                    var result = _connection.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return _connection;
        }
    }
}
