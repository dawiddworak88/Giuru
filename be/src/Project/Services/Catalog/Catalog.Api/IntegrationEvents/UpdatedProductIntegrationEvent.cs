using Foundation.EventBus.Events;
using System;

namespace Catalog.Api.IntegrationEvents
{
    public class UpdatedProductIntegrationEvent : IntegrationEvent
    {
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
    }
}
