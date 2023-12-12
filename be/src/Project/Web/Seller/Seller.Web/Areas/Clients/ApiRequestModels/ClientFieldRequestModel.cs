using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientFieldRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
