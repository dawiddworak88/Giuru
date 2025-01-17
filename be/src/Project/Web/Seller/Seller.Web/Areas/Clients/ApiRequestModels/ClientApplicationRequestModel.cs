using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientApplicationRequestModel : RequestModelBase
    {
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactJobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CommunicationLanguage { get; set; }
        public bool IsDeliveryAddressEqualBillingAddress { get; set; }
        public ClientApplicationAddressRequestModel BillingAddress { get; set; }
        public ClientApplicationAddressRequestModel DeliveryAddress { get; set; }
    }
}
