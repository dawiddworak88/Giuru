using Foundation.EventBus.Events;
using System;

namespace Ordering.Api.v1.Areas.Orders.Events
{
    public class BasketCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
    }
}
