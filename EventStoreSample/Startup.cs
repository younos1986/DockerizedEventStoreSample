using EventStoreSample.Events;
using EventStoreSample.EventStoreConfig;
using EventStoreSample.MongoDbConfigs;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventStoreSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>());


            
            services.AddSingleton<IMongoDb, MongoDb>();

            services.AddSingleton<IEventStoreDbContext, EventStoreDbContext>();
            services.AddMediatR(typeof(CustomerCreatedEvent).Assembly);
            services.AddSingleton<IEventStoreDbContext, EventStoreDbContext>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EventLoggerBehavior<,>));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
