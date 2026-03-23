using Foundation.EventBus.Events;

namespace Inventory.Api.IntegrationEvents
{
    public class UpsertedInventoryAvailabiIityIntegrationEvent : IntegrationEvent
    {
        public string[] ProductSkus { get; set; }
    }
}
