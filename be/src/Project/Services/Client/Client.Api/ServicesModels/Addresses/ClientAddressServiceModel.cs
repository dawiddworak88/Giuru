using System;

namespace Client.Api.ServicesModels.Addresses
{
    public class ClientAddressServiceModel
    {
        public Guid? Id { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public string Company {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? CountryId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
