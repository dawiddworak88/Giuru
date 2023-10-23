using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Addresses
{
    public class UpdateClientAddressServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? ClientId { get; set; }
        public string Recipient { get; set; }
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? CountryId { get; set; }
    }
}
