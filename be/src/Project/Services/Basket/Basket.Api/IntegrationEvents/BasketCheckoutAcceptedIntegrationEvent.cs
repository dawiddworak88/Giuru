using Basket.Api.IntegrationEventsModels;
using Foundation.EventBus.Events;
using System;

namespace Basket.Api.IntegrationEvents
{
    public class BasketCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
        public Guid? SellerId { get; set; }
        public BasketEventModel Basket { get; set; }
    }
}
