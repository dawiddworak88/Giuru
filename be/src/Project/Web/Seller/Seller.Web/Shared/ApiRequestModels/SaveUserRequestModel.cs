using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class SaveUserRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public string ReturnUrl { get; set; }
    }
}
