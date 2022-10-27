using System;

namespace Analytics.Api.v1.ResponseModels
{
    public class TopSalesProductsAnalyticsResponseModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string Ean { get; set; }
        public double Quantity { get; set; }
    }
}
