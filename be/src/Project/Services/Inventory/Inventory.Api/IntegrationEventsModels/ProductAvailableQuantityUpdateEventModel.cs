using System;

namespace Inventory.Api.IntegrationEventsModels
{
    public class ProductAvailableQuantityUpdateEventModel
    {
        public string ProductSku { get; set; }
        public double AvailableQuantity { get; set; }
    }
}
