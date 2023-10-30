using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Addresses
{
    public class CreateClientAddressServiceModel : BaseServiceModel
    {
        public Guid? ClientId { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? CountryId { get; set; }
    }
}
