using Foundation.EventBus.Events;
using System;
using System.Collections.Generic;

namespace Inventory.Api.IntegrationEvents
{
    public class ProductsSoldOutIntegrationEvent : IntegrationEvent
    {
        public IEnumerable<Guid> SoldOutProductIds { get; set; }
    }
}
