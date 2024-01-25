using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class UpdateClientRequestModel : RequestModelBase
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string CommunicationLanguage { get; set; }
        public bool IsDisabled { get; set; }
    }
}
