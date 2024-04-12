using System;

namespace Ordering.Api.ServicesModels.OrderAttributeValues
{
    public class CheckoutBasketAttributeValueServiceModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
