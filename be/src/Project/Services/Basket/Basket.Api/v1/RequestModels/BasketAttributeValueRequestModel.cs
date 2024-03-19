using System;

namespace Basket.Api.v1.RequestModels
{
    public class BasketAttributeValueRequestModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
