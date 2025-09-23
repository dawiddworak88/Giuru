using Catalog.BackgroundTasks.IntegrationEventModels;
using Foundation.EventBus.Events;
using System;
using System.Collections.Generic;

namespace Catalog.BackgroundTasks.IntegrationEvents
{
    public class OutletProductsAvailableQuantityUpdateIntegrationEvent : IntegrationEvent
    {
        public IEnumerable<ProductAvailableQuantityUpdateEventModel> Products { get; set; }
    }
}
