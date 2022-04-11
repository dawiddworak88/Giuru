using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels
{
    public class CreateClientServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ClientOrganisationId { get; set; }
    }
}
