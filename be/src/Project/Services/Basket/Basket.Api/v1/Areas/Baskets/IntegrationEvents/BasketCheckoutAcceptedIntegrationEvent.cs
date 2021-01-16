using Basket.Api.v1.Areas.Baskets.Models;
using Foundation.EventBus.Events;
using System;

namespace Basket.Api.v1.Areas.Baskets.IntegrationEvents
{
    public class BasketCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
        public Guid? SellerId { get; set; }
        public BasketModel Basket { get; set; }
    }
}
