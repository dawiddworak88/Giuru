using Foundation.ApiExtensions.Models.Request;

namespace Ordering.Api.v1.RequestModels
{
    public class OrderAttributeRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOrderItemAttribute { get; set; }
    }
}
