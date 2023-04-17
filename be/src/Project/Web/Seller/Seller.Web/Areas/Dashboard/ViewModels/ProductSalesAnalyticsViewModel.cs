using System;

namespace Seller.Web.Areas.Dashboard.ViewModels
{
    public class ProductSalesAnalyticsViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public double Quantity { get; set; }
    }
}
