using Foundation.EventBus.Events;
using System;

namespace Catalog.BackgroundTasks.IntegrationEvents
{
    public class RebuildCategoryProductsIntegrationEvent : IntegrationEvent
    {
        public Guid? CategoryId { get; set; }
    }
}
