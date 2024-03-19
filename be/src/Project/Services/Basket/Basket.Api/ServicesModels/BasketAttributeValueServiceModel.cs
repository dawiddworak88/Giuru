using System;

namespace Basket.Api.ServicesModels
{
    public class BasketAttributeValueServiceModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
