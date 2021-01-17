using Foundation.EventBus.Events;
using System;

namespace Basket.Api.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid? BasketId { get; set; }
    }
}
