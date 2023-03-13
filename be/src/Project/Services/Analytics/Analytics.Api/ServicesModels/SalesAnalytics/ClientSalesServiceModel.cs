using System;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class ClientSalesServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
    }
}
