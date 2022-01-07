using System;

namespace Buyer.Web.Areas.Products.DomainModels
{
    public class InventorySum
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int? Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        
    }
}
