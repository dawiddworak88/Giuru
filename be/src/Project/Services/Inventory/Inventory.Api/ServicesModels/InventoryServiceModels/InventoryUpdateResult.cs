using System;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class InventoryUpdateResult
    {
        public Guid ProductId { get; set; }
        public double PreviousQuantity { get; set; }
        public double NewQuantity { get; set; }
        public bool IsWentOutOfStock { get; set; }
    }
}
