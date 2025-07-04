using System;

namespace Buyer.Web.Shared.DomainModels.Clients
{
    public class Client
    {
        public Guid Id { get; set; }
        public Guid? PreferedCurrencyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public Guid? DefaultDeliveryAddressId { get; set; }
        public Guid? DefaultBillingAddressId { get; set; }
        public Guid? CountryId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
