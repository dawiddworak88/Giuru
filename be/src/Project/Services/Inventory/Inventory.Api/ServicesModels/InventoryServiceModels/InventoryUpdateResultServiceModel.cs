using System;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class InventoryUpdateResultServiceModel
    {
        public Guid ProductId { get; set; }
        public double PreviousQuantity { get; set; }
        public double NewQuantity { get; set; }
        public bool WentOutOfStock { get; set; }
    }
}
