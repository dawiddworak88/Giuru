using Basket.Api.IntegrationEvents;
using Basket.Api.Repositories;
using Foundation.EventBus.Abstractions;
using System.Threading.Tasks;

namespace Basket.Api.IntegrationEventsHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IBasketRepository repository;

        public OrderStartedIntegrationEventHandler(
            IBasketRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(OrderStartedIntegrationEvent @event)
        {
            await this.repository.DeleteBasketAsync(@event.BasketId);
        }
    }
}
