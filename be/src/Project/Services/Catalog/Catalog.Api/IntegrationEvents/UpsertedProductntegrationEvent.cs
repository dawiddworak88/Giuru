using Foundation.EventBus.Events;
using System;

namespace Catalog.Api.IntegrationEvents
{
    public class UpsertedProductntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; set; }
        public string ProductSku { get; set; }
    }
}
