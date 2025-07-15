using System;

namespace Inventory.Api.IntegrationEventsModels
{
    public class ProductAvailableQuantityUpdateEventModel
    {
        public Guid Id { get; set; }
        public double AvailableQuantity { get; set; }
    }
}
