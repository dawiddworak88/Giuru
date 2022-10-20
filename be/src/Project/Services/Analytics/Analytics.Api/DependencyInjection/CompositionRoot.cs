using Analytics.Api.Infrastructure;
using Analytics.Api.Services.Products;
using Analytics.Api.Services.SalesAnalytics;
using Foundation.EventBus.Abstractions;
using Foundation.EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Reflection;
using System;
using Analytics.Api.IntegrationEvents;
using Analytics.Api.IntegrationEventsHandlers;
using Foundation.EventBusRabbitMq;

namespace Analytics.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAnalyticsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IProductService, ProductService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AnalyticsContext>();

            services.AddDbContext<AnalyticsContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite().MigrationsAssembly("Analytics.Api")));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<UpdatedProductIntegrationEvent>, UpdatedProductIntegrationEventHandler>();

            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(configuration["EventBusConnection"]),
                    DispatchConsumersAsync = true
                };

                factory.RequestedHeartbeat = TimeSpan.FromSeconds(int.Parse(configuration["EventBusRequestedHeartbeat"]));

                return new DefaultRabbitMQPersistentConnection(factory, logger, int.Parse(configuration["EventBusRetryCount"]));
            });

            services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMq(sp, rabbitMqPersistentConnection, logger, eventBusSubcriptionsManager, Assembly.GetExecutingAssembly().GetName().Name, int.Parse(configuration["EventBusRetryCount"]));
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
