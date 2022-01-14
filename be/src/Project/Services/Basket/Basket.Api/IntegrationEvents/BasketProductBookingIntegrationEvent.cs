using Foundation.EventBus.Events;
using System;

namespace Basket.Api.IntegrationEvents
{
    public class BasketProductBookingIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; set; }
        public string ProductSku { get; set; }
        public int BookedQuantity { get; set; }
    }
}
