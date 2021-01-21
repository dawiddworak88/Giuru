using Foundation.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Api.IntegrationEvents.Events;
using Serilog.Context;
using System.Threading.Tasks;

namespace Ordering.Api.v1.Areas.Orders.IntegrationEvents.EventHandlers
{
    public class BasketCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>
    {
        private readonly IMediator mediator;
        private readonly ILogger<BasketCheckoutAcceptedIntegrationEventHandler> logger;

        public BasketCheckoutAcceptedIntegrationEventHandler(
            IMediator mediator,
            ILogger<BasketCheckoutAcceptedIntegrationEventHandler> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
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
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{typeof(Program).Namespace}"))
            {
                this.logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, typeof(Program).Namespace, @event);
            }
        }
    }
}
