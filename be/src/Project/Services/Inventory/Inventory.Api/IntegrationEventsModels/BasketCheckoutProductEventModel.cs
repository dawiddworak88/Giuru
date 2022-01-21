using System;

namespace Inventory.Api.IntegrationEventsModels
{
    public class BasketCheckoutProductEventModel
    {
        public Guid? ProductId { get; set; }
        public int BookedQuantity { get; set; }
    }
}
