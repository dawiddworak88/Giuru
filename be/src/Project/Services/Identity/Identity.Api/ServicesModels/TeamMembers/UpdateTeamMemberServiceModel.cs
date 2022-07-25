using Foundation.Extensions.Models;

namespace Identity.Api.ServicesModels.TeamMembers
{
    public class UpdateTeamMemberServiceModel : BaseServiceModel 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
