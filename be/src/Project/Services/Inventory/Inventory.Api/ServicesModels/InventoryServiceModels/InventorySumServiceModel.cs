using System;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class InventorySumServiceModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string ProductEan { get; set; }
        public double? Quantity { get; set; }
        public double? AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public IEnumerable<InventoryServiceModel> Details { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
    }
}
