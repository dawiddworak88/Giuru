namespace Buyer.Web.Shared.ApiRequestModels.Application
{
    public class ClientApplicationAddressRequestModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
