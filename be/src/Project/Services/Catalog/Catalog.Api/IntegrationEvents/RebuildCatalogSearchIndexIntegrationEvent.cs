using Foundation.EventBus.Events;
using System;

namespace Catalog.Api.IntegrationEvents
{
    public class RebuildCatalogSearchIndexIntegrationEvent : IntegrationEvent
    {
        public Guid? SellerId { get; set; }
    }
}
