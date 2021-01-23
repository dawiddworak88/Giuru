using Foundation.EventBus.Events;
using System;

namespace Ordering.Api.IntegrationEvents
{
    public class BasketCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
    }
}
