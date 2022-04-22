using Basket.Api.IntegrationEventsModels;
using Foundation.EventBus.Events;
using System.Collections.Generic;

namespace Basket.Api.IntegrationEvents
{
    public class BasketCheckoutStockProductsIntegrationEvent : IntegrationEvent
    {
        public IEnumerable<BasketCheckoutProductEventModel> Items { get; set; }
    }
}
