using System;

namespace Seller.Web.Areas.Dashboard.DomainModels
{
    public class ProductSalesItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public double Quantity { get; set; }
    }
}
