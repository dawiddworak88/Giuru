using Basket.Api.IntegrationEvents;
using Basket.Api.Repositories;
using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Definitions;
using Foundation.EventLog.Repositories;
using System.Threading.Tasks;

namespace Basket.Api.IntegrationEventsHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IBasketRepository repository;
        private readonly IEventLogRepository eventLogRepository;

        public OrderStartedIntegrationEventHandler(
            IBasketRepository repository,
            IEventLogRepository eventLogRepository)
        {
            this.repository = repository;
            this.eventLogRepository = eventLogRepository;
        }

        public async Task Handle(OrderStartedIntegrationEvent @event)
        {
            await this.eventLogRepository.SaveAsync(@event, EventStates.InProgress);
            
            await this.repository.DeleteBasketAsync(@event.BasketId);

            await this.eventLogRepository.SaveAsync(@event, EventStates.Processed);
        }
    }
}
