using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventStoreSample.MongoDbConfigs
{
    public interface IMongoDb
    {
        Task InsertOneAsync<T>(T entity);
    }
}
