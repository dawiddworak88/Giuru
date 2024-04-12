using System;

namespace Ordering.Api.IntegrationEventsModels
{
    public class BasketAttributeValueEventModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
