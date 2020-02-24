using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EventStore.Subscriber
{
    /*
      * This example sets up a persistent subscription to a test stream.
      * 
      * As written it will use the default ipaddress (loopback) and the default tcp port 1113 of the event
      * store. In order to run the application bring up the event store in another window (you can use
      * default arguments eg EventStore.ClusterNode.exe) then you can run this application with it. Once 
      * this program is running you can run the WritingEvents sample to write some events to the stream
      * and they will appear over the persistent subscription. You can also run many concurrent instances of this
      * program and only one instance will receive the event.
      * 
      */

    internal class Program
    {
        private static void Main()
        {

            //var sth = new IPEndPoint(IPAddress.Parse("172.16.0.13"), 1113);

            var subscription = new PersistentSubscriptionClient();
            subscription.Start();
        }
    }

    public class PersistentSubscriptionClient
    {
        private IEventStoreConnection _conn;
        private const string STREAM = "EventStoreSample";
        private const string GROUP = "EventStoreSample";
        private const int DEFAULTPORT = 1113;
        private static readonly UserCredentials User = new UserCredentials("admin", "changeit");
        private EventStorePersistentSubscriptionBase _subscription;

        public void Start()
        {
            //uncommet to enable verbose logging in client.
            //var settings = ConnectionSettings.Create(); //.EnableVerboseLogging().UseConsoleLogger();

            //var ip = Dns.GetHostAddresses("eventstore").Where(a => a.AddressFamily == AddressFamily.InterNetwork).First();
            //var connectionString = $"ConnectTo=tcp://admin:changeit@{ip}:1113";

            //using (_conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, DEFAULTPORT)))
            //using (_conn = EventStoreConnection.Create(settings, new IPEndPoint(ip, DEFAULTPORT)))
            //using (_conn = EventStoreConnection.Create(settings, connectionString))
            using (_conn = EventStoreConnection.Create("ConnectTo=tcp://admin:changeit@eventstore:1113"))
            {
                _conn.ConnectAsync().Wait();

                CreateSubscription();
                ConnectToSubscription();

                Console.WriteLine("waiting for events. press enter to exit");
                Console.ReadLine();
            }
        }


        private void ConnectToSubscription()
        {
            var bufferSize = 10;
            var autoAck = true;

            _subscription = _conn.ConnectToPersistentSubscription(STREAM, GROUP, EventAppeared, SubscriptionDropped,
                User, bufferSize, autoAck);
        }

        private void SubscriptionDropped(EventStorePersistentSubscriptionBase eventStorePersistentSubscriptionBase,
            SubscriptionDropReason subscriptionDropReason, Exception ex)
        {
            ConnectToSubscription();
        }

        private static void EventAppeared(EventStorePersistentSubscriptionBase eventStorePersistentSubscriptionBase,
            ResolvedEvent resolvedEvent)
        {
            ;
            var data = Encoding.ASCII.GetString(resolvedEvent.Event.Data);
            Console.WriteLine("Received: " + resolvedEvent.Event.EventStreamId + ":" + resolvedEvent.Event.EventNumber);
            Console.WriteLine(data);


            //await _mongoDb.InsertOneAsync(customer);
        }

        /*
        * Normally the creating of the subscription group is not done in your general executable code. 
        * Instead it is normally done as a step during an install or as an admin task when setting 
        * things up. You should assume the subscription exists in your code.
        */

        private void CreateSubscription()
        {
            PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();

            try
            {
                _conn.CreatePersistentSubscriptionAsync(STREAM, GROUP, settings, User).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType() != typeof(InvalidOperationException)
                    && ex.InnerException?.Message != $"Subscription group {GROUP} on stream {STREAM} already exists")
                {
                    throw;
                }
            }
        }
    }
}
