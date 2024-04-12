using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class OrderAttributeOptionRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public Guid? AttributeId { get; set; }
    }
}
