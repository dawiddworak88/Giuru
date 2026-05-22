using Foundation.EventBus.Events;

namespace Inventory.Api.IntegrationEvents
{
    public class UpsertedInventoryAvailabilityIntegrationEvent : IntegrationEvent
    {
        public string[] ProductSkus { get; set; }
    }
}
