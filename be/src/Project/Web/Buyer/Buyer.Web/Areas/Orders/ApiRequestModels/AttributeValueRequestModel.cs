using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class AttributeValueRequestModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
