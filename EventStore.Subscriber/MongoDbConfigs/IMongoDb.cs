using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventStore.Subscriber.MongoDbConfigs
{
    public interface IMongoDb
    {
        Task InsertOneAsync<T>(T entity);
    }
}
