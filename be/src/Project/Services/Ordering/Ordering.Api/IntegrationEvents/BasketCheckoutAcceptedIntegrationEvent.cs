using Foundation.EventBus.Events;
using Ordering.Api.IntegrationEventsModels;
using System;

namespace Ordering.Api.IntegrationEvents
{
    public class BasketCheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
        public Guid? SellerId { get; set; }
        public Guid? BillingAddressId { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string MoreInfo { get; set; }
        public BasketEventModel Basket { get; set; }
    }
}
