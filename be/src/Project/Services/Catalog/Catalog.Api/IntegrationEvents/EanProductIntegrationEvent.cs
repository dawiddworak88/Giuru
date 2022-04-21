using Foundation.EventBus.Events;
using System;

namespace Catalog.Api.IntegrationEvents
{
    public class EanProductIntegrationEvent : IntegrationEvent
    {
        public Guid? ProductId { get; set; }
        public string Ean { get; set; }
    }
}
