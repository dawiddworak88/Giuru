using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Applications
{
    public class UpdateClientApplicationServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactJobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CommunicationLanguage { get; set; }
        public bool IsDeliveryAddressEqualBillingAddress { get; set; }
        public ClientApplicationAddressServiceModel BillingAddress { get; set; }
        public ClientApplicationAddressServiceModel DeliveryAddress { get; set; }
    }
}
