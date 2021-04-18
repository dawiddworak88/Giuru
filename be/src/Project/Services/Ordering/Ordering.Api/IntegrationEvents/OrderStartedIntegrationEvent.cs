using Foundation.EventBus.Events;
using System;

namespace Ordering.Api.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid? BasketId { get; set; }
    }
}
