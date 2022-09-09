using Foundation.ApiExtensions.Models.Request;

namespace Identity.Api.v1.RequestModels
{
    public class TeamMemberRequestModel : RequestModelBase
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }
}
