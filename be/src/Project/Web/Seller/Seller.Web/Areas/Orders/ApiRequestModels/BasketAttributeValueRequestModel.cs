using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class BasketAttributeValueRequestModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
