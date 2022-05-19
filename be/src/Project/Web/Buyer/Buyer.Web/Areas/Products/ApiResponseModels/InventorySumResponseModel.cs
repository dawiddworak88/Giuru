using System;

namespace Buyer.Web.Areas.Products.DomainModels
{
    public class InventorySumResponseModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public double? Quantity { get; set; }
        public double? AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        
    }
}
