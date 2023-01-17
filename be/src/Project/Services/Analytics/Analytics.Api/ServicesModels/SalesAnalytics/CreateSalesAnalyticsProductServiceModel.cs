using System;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class CreateSalesAnalyticsProductServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public bool IsStock { get; set; }
        public bool IsOutlet { get; set; }
        public double Quantity { get; set; }
    }
}
