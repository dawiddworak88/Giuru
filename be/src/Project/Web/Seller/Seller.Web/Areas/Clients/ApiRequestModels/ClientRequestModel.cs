using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
    }
}
