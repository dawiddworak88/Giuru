using System;

namespace Basket.Api.IntegrationEventsModels
{
    public class BasketCheckoutProductEventModel
    {
        public Guid? ProductId { get; set; }
        public double BookedQuantity { get; set; }
    }
}
