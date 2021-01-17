using Basket.Api.IntegrationEvents.Events;
using Basket.Api.v1.Areas.Baskets.Repositories;
using Foundation.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading.Tasks;

namespace Basket.Api.IntegrationEvents.EventHandling
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IBasketRepository repository;
        private readonly ILogger<OrderStartedIntegrationEventHandler> logger;

        public OrderStartedIntegrationEventHandler(
            IBasketRepository repository,
            ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task Handle(OrderStartedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{typeof(Program).Namespace}"))
            {
                logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, typeof(Program).Namespace, @event);

                await repository.DeleteBasketAsync(@event.BasketId);
            }
        }
    }
}
