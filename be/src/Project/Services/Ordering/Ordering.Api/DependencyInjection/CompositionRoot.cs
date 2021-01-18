using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Api.v1.Areas.Orders.Events;
using Ordering.Api.v1.Areas.Orders.IntegrationEvents;

namespace Ordering.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>, BasketCheckoutAcceptedIntegrationEventHandler>();

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
