using Basket.Api.IntegrationEventsModels;
using Foundation.EventBus.Events;
using System.Collections.Generic;

namespace Basket.Api.IntegrationEvents
{
    public class BasketCheckoutOutletProductsIntegrationEvent : IntegrationEvent
    {
        public IEnumerable<BasketCheckoutProductEventModel> Items { get; set; }
    }
}
