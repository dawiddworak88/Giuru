using System;

namespace Buyer.Web.Areas.Products.DomainModels
{
    public class ProductStock
    {
        public int? AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public string Title { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
    }
}
