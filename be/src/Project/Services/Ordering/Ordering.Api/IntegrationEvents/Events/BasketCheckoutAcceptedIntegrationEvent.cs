using Foundation.EventBus.Events;
using System;

namespace Ordering.Api.IntegrationEvents.Events
{
    public class BasketCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
    }
}
