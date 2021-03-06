using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels
{
    public class UpdateClientServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public Guid? ClientOrganisationId { get; set; }
    }
}
