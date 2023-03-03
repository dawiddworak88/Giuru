using System;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class CreateSalesAnalyticsProductServiceModel
    {
        public Guid? Id { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
    }
}
