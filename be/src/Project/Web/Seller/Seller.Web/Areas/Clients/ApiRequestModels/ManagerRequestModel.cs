using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ManagerRequestModel : RequestModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
