using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventStoreSample.EventStoreConfig
{
    public interface IEventStoreDbContext
    {
        Task<IEventStoreConnection> GetConnection();

        Task AppendToStreamAsync(params EventData[] events);
    }
}
