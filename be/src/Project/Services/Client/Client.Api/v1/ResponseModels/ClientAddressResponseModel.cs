using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientAddressResponseModel
    {
        public Guid? Id { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
