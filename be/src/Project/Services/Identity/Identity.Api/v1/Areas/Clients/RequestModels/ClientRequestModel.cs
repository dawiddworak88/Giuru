using Foundation.ApiExtensions.Models.Request;

namespace Identity.Api.v1.Areas.Clients.RequestModels
{
    public class ClientRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
    }
}
