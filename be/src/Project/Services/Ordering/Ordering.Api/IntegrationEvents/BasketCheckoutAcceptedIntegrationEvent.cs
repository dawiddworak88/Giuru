using Foundation.EventBus.Events;
using Ordering.Api.IntegrationEventsModels;
using System;

namespace Ordering.Api.IntegrationEvents
{
    public class BasketCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
        public Guid? SellerId { get; set; }
        public BasketEventModel Basket { get; set; }
    }
}
