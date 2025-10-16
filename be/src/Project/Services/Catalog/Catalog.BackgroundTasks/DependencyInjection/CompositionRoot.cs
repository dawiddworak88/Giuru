using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.IntegrationEventsHandlers;
using Catalog.BackgroundTasks.Services.CategorySchemas;
using Catalog.BackgroundTasks.Services.Products;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Repositories.ProductSearchRepositories;
using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Foundation.Localization.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using RabbitMQ.Client;
using System;
using System.Reflection;

namespace Catalog.BackgroundTasks.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterCatalogBackgroundTasksDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ICategorySchemaService, CategorySchemaService>();
            services.AddScoped<IProductSearchRepository, ProductSearchRepository>();
        }

        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalizationSettings>(configuration);
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CatalogContext>();

            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterSearchDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticsearchUrl"];
            var defaultIndex = configuration["ElasticsearchIndex"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultDisableIdInference();

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<RebuildCatalogSearchIndexIntegrationEvent>, RebuildCatalogSearchIndexIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RebuildCategorySchemasIntegrationEvent>, RebuildCategorySchemasIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RebuildCategoryProductsIntegrationEvent>, RebuildCategoryProductsIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<InventoryProductsAvailableQuantityUpdateIntegrationEvent>, InventoryProductsAvailableQuantityUpdateIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<OutletProductsAvailableQuantityUpdateIntegrationEvent>, OutletProductsAvailableQuantityUpdateIntegrationEventHandler>();

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
