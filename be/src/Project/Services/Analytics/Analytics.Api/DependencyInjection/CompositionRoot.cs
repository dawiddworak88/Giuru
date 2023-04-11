using Analytics.Api.Infrastructure;
using Analytics.Api.Services.Products;
using Analytics.Api.Services.SalesAnalytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System;
using Foundation.EventBusRabbitMq;
using RabbitMQ.Client;
using Foundation.EventBus.Abstractions;
using Foundation.EventBus;
using Analytics.Api.IntegrationEvents;
using Analytics.Api.IntegrationEventHandlers;
using Analytics.Api.Repositories.Clients;
using Analytics.Api.Repositories.Global;
using Analytics.Api.Repositories.Products;
using Analytics.Api.Repositories.AccessToken;

namespace Analytics.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAnalyticsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AnalyticsContext>();

            services.AddDbContext<AnalyticsContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<OrderStartedIntegrationEvent>, OrderStartedIntegrationEventHandler>();

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
