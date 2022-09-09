using Foundation.ApiExtensions.Models.Response;

namespace Identity.Api.v1.ResponseModels
{
    public class TeamMemberResponseModel : BaseResponseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
