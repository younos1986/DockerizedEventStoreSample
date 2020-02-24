using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace EventStore.Subscriber.MongoDbConfigs
{
    public class MongoDb : IMongoDb
    {
        private IMongoCollection<T> GetCollection<T>()
        {
            var _connectionString = "mongodb://test:test@mongodb/admin?authSource=admin";
            var _databaseName = MongoUrl.Create(_connectionString).DatabaseName;
            //MongoClient dbClient = new MongoClient(_connectionString).GetDatabase(_databaseName);
            MongoClient dbClient = new MongoClient(_connectionString);


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
