using System;

namespace Ordering.Api.ServicesModels
{
    public class CheckoutBasketAttributeValueServiceModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
