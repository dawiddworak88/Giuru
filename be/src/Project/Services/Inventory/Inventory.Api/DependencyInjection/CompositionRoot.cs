using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Inventory.Api.Infrastructure;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.IntegrationEventsHandlers;
using Inventory.Api.Services.InventoryItems;
using Inventory.Api.Services.OutletItems;
using Inventory.Api.Services.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Reflection;

namespace Inventory.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterInventoryApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IOutletService, OutletService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<InventoryContext>();
            services.AddDbContext<InventoryContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<UpdatedProductIntegrationEvent>, UpdatedProductIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<UpdatedEanProductIntegrationEvent>, UpdatedEanProductIntegrationEventHandler>();
            services.AddScoped<IIntegrationEventHandler<BasketCheckoutStockProductsIntegrationEvent>, UpdatedInventoryIntegrationEventHandler>();
            services.AddScoped <IIntegrationEventHandler<BasketCheckoutOutletProductsIntegrationEvent>,  UpdatedOutletIntegrationEventHandler > ();

            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(configuration["EventBusConnection"]),
                    DispatchConsumersAsync = true
                };

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
