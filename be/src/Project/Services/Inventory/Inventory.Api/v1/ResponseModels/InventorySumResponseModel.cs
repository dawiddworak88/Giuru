using System;
using System.Collections.Generic;

namespace Inventory.Api.v1.ResponseModels
{
    public class InventorySumResponseModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string Ean { get; set; }
        public int? Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public IEnumerable<InventoryDetailsResponseModel> Details { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        
    }
}
