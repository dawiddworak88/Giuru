using System;

namespace Basket.Api.IntegrationEventsModels
{
    public class BasketAttributeValueEventModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
