using Foundation.EventBus.Events;
using System;

namespace Inventory.Api.IntegrationEvents
{
    public class EanProductIntegrationEvent : IntegrationEvent
    {
        public Guid? ProductId { get; set; }
        public string Ean { get; set; }
    }
}
