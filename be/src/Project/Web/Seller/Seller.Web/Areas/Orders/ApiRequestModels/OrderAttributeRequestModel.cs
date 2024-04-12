using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class OrderAttributeRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsOrderItemAttribute { get; set; }
    }
}
