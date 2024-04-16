using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class ClientApplication
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactJobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CommunicationLanguage { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCountry { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyRegion { get; set; }
        public string CompanyPostalCode { get; set; }
        public bool IsDeliveryAddressEqualBillingAddress { get; set; }
        public ClientApplicationAddress BillingAddress { get; set; }
        public ClientApplicationAddress DeliveryAddress { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
