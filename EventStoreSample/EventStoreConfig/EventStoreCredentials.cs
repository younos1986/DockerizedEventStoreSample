using EventStore.ClientAPI.SystemData;

namespace EventStoreSample.EventStoreConfig
{
    public class EventStoreCredentials
    {
        public static UserCredentials Default { get; } = new UserCredentials("admin", "changeit");
    }
}
