using Foundation.ApiExtensions.Models.Request;
using System;

namespace Client.Api.v1.RequestModels
{
    public class ClientRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
