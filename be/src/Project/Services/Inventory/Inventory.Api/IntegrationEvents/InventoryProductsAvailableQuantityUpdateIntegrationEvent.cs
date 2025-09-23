using Foundation.EventBus.Events;
using Inventory.Api.IntegrationEventsModels;
using System.Collections.Generic;

namespace Inventory.Api.IntegrationEvents
{
    public class InventoryProductsAvailableQuantityUpdateIntegrationEvent : IntegrationEvent
    {
        public IEnumerable<ProductAvailableQuantityUpdateEventModel> Products { get; set; }
    }
}
