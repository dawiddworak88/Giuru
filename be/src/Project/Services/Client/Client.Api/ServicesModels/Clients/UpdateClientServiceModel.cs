using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.Clients
{
    public class UpdateClientServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? ClientOrganisationId { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public IEnumerable<Guid> ClientManagerIds { get; set; }
        public Guid? DefaultDeliveryAddressId { get; set; }
        public Guid? DefaultBillingAddressId { get; set; }
    }
}
