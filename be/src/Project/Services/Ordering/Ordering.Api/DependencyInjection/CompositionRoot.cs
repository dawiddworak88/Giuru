using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Api.Infrastructure;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.Services.OrderAttributeOptions;
using Ordering.Api.Services.OrderAttributes;
using Ordering.Api.Services.OrderAttributeValues;
using Ordering.Api.Services.Orders;
using Ordering.Api.v1.Areas.Orders.IntegrationEventsHandlers;
using System.Reflection;

namespace Ordering.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrderingApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IOrderAttributesService, OrderAttributesService>();
            services.AddScoped<IOrderAttributeOptionsService, OrderAttributeOptionsService>();
            services.AddScoped<IOrderAttributeValuesService, OrderAttributeValuesService>();
        }

        public static void RegisterOrderingDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<OrderingContext>();

            services.AddDbContext<OrderingContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>, BasketCheckoutAcceptedIntegrationEventHandler>();

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
