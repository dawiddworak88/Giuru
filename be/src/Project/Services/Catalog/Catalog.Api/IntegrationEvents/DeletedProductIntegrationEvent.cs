using Foundation.EventBus.Events;
using System;

namespace Catalog.Api.IntegrationEvents
{
    public class DeletedProductIntegrationEvent : IntegrationEvent
    {
        public Guid? ProductId { get; set; }
    }
}
