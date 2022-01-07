using Foundation.ApiExtensions.Models.Request;

namespace Identity.Api.v1.RequestModels
{
    public class OrganisationRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
    }
}
