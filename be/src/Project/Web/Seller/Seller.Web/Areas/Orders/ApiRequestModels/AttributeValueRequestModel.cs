using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class AttributeValueRequestModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
