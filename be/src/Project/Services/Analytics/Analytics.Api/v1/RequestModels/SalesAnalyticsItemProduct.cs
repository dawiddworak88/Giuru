using System;

namespace Analytics.Api.v1.RequestModels
{
    public class SalesAnalyticsItemProduct
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
