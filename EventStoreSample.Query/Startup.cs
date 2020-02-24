using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventStoreSample.Query
{
    public class Startup
    {
        const string STREAM = "EventStoreSample";
        const string GROUP = "EventStoreSample";
        const int DEFAULTPORT = 1113;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();





            //uncommet to enable verbose logging in client.
            var settings = ConnectionSettings.Create();//.EnableVerboseLogging().UseConsoleLogger();
            using (var conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, DEFAULTPORT)))
            {
                conn.ConnectAsync().Wait();

                //Normally the creating of the subscription group is not done in your general executable code. 
                //Instead it is normally done as a step during an install or as an admin task when setting 
                //things up. You should assume the subscription exists in your code.
                CreateSubscription(conn);

                conn.ConnectToPersistentSubscription(STREAM, GROUP, (_, x) =>
                {
                    var data = Encoding.ASCII.GetString(x.Event.Data);
                    Console.WriteLine("Received: " + x.Event.EventStreamId + ":" + x.Event.EventNumber);
                    Console.WriteLine(data);
                });

                Console.WriteLine("waiting for events. press enter to exit");
                Console.ReadLine();
            }





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private static void CreateSubscription(IEventStoreConnection conn)
        {
            PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();

            try
            {
                conn.DeletePersistentSubscriptionAsync(STREAM, GROUP, new UserCredentials("admin", "changeit")).Wait();
                conn.CreatePersistentSubscriptionAsync(STREAM, GROUP, settings, new UserCredentials("admin", "changeit")).Wait();
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
