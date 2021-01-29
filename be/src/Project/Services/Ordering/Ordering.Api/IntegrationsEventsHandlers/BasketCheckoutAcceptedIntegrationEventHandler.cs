using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Definitions;
using Foundation.EventLog.Repositories;
using Ordering.Api.IntegrationEvents;
using System.Threading.Tasks;

namespace Ordering.Api.v1.Areas.Orders.IntegrationEventsHandlers
{
    public class BasketCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>
    {
        private readonly IEventLogRepository eventLogRepository;

        public BasketCheckoutAcceptedIntegrationEventHandler(
            IEventLogRepository eventLogRepository)
        {
            this.eventLogRepository = eventLogRepository;
        }

        /// <summary>
        /// Integration event handler which starts the create order process
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// basket.api once it has successfully process the 
        /// order items.
        /// </param>
        /// <returns></returns>
        public async Task Handle(BasketCheckoutAcceptedIntegrationEvent @event)
        {
            await this.eventLogRepository.SaveAsync(@event, @event.GetType().Name, EventStates.InProgress, @event.Source, @event.IpAddress);

            await this.eventLogRepository.SaveAsync(@event, @event.GetType().Name, EventStates.Processed, @event.Source, @event.IpAddress);
        }
    }
}
