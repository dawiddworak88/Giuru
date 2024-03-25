
using Catalog.Api.Services.Categories;
using Catalog.Api.Services.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Foundation.Catalog.Infrastructure;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Microsoft.Extensions.Logging;
using Foundation.EventBus;
using RabbitMQ.Client;
using Catalog.Api.Services.ProductAttributes;
using System.Reflection;
using Foundation.Catalog.Repositories.ProductSearchRepositories;

namespace Catalog.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterCatalogApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();

            services.AddScoped<IProductSearchRepository, ProductSearchRepository>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IProductAttributesService, ProductAttributesService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CatalogContext>();

            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite().MigrationsAssembly("Catalog.Api")));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
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
