using Foundation.ApiExtensions.Models;
using Foundation.ApiExtensions.Models.Request;
using System;

namespace Client.Api.v1.RequestModels
{
    public class ClientRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
    }
}
