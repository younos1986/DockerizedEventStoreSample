using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using MediatR;
using Newtonsoft.Json;

namespace EventStoreSample.EventStoreConfig
{
    public class EventLoggerBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
    {
        readonly IEventStoreDbContext _eventStoreDbContext;

        public EventLoggerBehavior(IEventStoreDbContext eventStoreDbContext)
        {
            _eventStoreDbContext = eventStoreDbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = await next();

            string requestName = request.ToString();

            if (requestName.EndsWith("Command"))
            {
                Type requestType = request.GetType();
                string commandName = requestType.Name;

                var data = new Dictionary<string, object>
                {
                    {
                        "request", request
                    },
                    {
                        "response", response
                    }
                };

                string jsonData = JsonConvert.SerializeObject(data);
                byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

                EventData eventData = new EventData(eventId: Guid.NewGuid(),
                    type: commandName,
                    isJson: true,
                    data: dataBytes,
                    metadata: null);

                await _eventStoreDbContext.AppendToStreamAsync(eventData);
            }

            return response;
        }
    }
}
