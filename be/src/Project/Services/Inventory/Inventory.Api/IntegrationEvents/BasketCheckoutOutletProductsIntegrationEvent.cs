using Foundation.EventBus.Events;
using Inventory.Api.IntegrationEventsModels;
using System.Collections.Generic;

namespace Inventory.Api.IntegrationEvents
{
    public class BasketCheckoutOutletProductsIntegrationEvent : IntegrationEvent
    {
        public IEnumerable<BasketCheckoutProductEventModel> Items { get; set; }
    }
}
