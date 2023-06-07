using Foundation.EventBus.Events;
using System;

namespace Catalog.Api.IntegrationEvents
{
    public class RebuildCategoryProductsIntegrationEvent : IntegrationEvent
    {
        public Guid? CategoryId { get; set; }
    }
}
