using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class OrderAttributeValueRequestModel
    {
        public string Value { get; set; }
        public Guid? AttributeId { get; set; }
    }
}
