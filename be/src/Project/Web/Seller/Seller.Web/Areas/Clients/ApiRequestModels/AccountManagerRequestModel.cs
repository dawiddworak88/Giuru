using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class AccountManagerRequestModel : RequestModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
