using System;

namespace Analytics.Api.IntegrationEventsModels
{
    public class OrderItemStartedEventModel
    {
        public Guid? Id { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
    }
}
