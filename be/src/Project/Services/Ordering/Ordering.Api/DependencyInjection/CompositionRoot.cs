using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Api.Infrastructure.Auditing;
using Ordering.Api.Infrastructure.Ordering;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.v1.Areas.Orders.IntegrationEventsHandlers;

namespace Ordering.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrderingDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<OrderingContext>();

            services.AddDbContext<OrderingContext>(options => options.UseSqlServer(configuration["OrderingConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterAuditingDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditingContext>();

            services.AddDbContext<AuditingContext>(options => options.UseSqlServer(configuration["AuditingConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>, BasketCheckoutAcceptedIntegrationEventHandler>();

            services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMq(sp, rabbitMqPersistentConnection, logger, eventBusSubcriptionsManager, typeof(Startup).Namespace, int.Parse(configuration["EventBusRetryCount"]));
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
