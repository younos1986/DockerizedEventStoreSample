using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace EventStoreSample.MongoDbConfigs
{
    public class MongoDb : IMongoDb
    {
        private IMongoCollection<T> GetCollection<T>()
        {
            var server = new MongoServerAddress(host: "172.16.0.14", port: 27017);
            //var server = new MongoServerAddress(host: "localhost", port: 27014);
            var credentials = MongoCredential.CreateCredential(
                databaseName: "admin",
                username: "test",
                password: "test"
            );
            var mongodbClientSettings = new MongoClientSettings
            {
                Credential = credentials,
                Server = server,
                ConnectionMode = ConnectionMode.Standalone,
                ServerSelectionTimeout = TimeSpan.FromSeconds(3)
            };
            MongoClient dbClient = new MongoClient(mongodbClientSettings);

            var database = dbClient.GetDatabase("EventSampleApiDB");
            var collection = database.GetCollection<T>(typeof(T).Name);
            return collection;
        }

        public async Task InsertOneAsync<T>(T entity)
        {
            await GetCollection<T>().InsertOneAsync(entity);
        }

    }
}
