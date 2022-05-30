using Foundation.EventBus.Events;
using System;

namespace Inventory.Api.IntegrationEvents
{
    public class DeletedProductIntegrationEvent : IntegrationEvent
    {
        public Guid? ProductId { get; set; }
    }
}
