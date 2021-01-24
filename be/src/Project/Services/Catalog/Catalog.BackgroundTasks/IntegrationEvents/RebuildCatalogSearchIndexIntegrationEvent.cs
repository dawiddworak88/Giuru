using Foundation.EventBus.Events;
using System;

namespace Catalog.BackgroundTasks.IntegrationEvents
{
    public class RebuildCatalogSearchIndexIntegrationEvent : IntegrationEvent
    {
        public Guid? SellerId { get; set; }
    }
}
