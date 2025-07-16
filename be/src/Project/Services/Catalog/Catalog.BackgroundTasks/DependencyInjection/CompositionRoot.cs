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
using System.Text;

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
                .DefaultIndex(defaultIndex).DefaultDisableIdInference().EnableDebugMode() // włącza logowanie debugowe
    .PrettyJson()      // czytelniejszy JSON
    .OnRequestCompleted(details =>
    {
        Console.WriteLine("\n✅ REQUEST:");
        if (details.RequestBodyInBytes != null)
        {
            Console.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
        }

        Console.WriteLine($"\n➡️ METHOD: {details.HttpMethod}");
        Console.WriteLine($"➡️ URI: {details.Uri}");

        Console.WriteLine("\n📥 RESPONSE:");
        if (details.ResponseBodyInBytes != null)
        {
            Console.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
        }

        Console.WriteLine($"\n✅ STATUS: {details.HttpStatusCode}");
    });

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<RebuildCatalogSearchIndexIntegrationEvent>, RebuildCatalogSearchIndexIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RebuildCategorySchemasIntegrationEvent>, RebuildCategorySchemasIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<RebuildCategoryProductsIntegrationEvent>, RebuildCategoryProductsIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<InventoryProductsAvailableQuantityUpdateIntegrationEvent>, InventoryProductsAvailableQuantityUpdateIntegrationEventHandler>();

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
