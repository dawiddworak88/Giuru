using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;

namespace Identity.Api.ServicesModels.TeamMembers
{
    public class CreateTeamMemberServiceModel : BaseServiceModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Scheme { get; set; }
        public HostString Host { get; set; }
        public string ReturnUrl { get; set; }
    }
}
