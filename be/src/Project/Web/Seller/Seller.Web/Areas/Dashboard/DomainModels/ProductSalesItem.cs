using System;

namespace Seller.Web.Areas.Dashboard.DomainModels
{
    public class ProductSalesItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string Ean { get; set; }
        public double Quantity { get; set; }
    }
}
