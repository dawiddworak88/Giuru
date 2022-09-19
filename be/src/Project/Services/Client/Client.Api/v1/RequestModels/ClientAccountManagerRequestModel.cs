using Foundation.ApiExtensions.Models.Request;

namespace Client.Api.v1.RequestModels
{
    public class ClientAccountManagerRequestModel : RequestModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
