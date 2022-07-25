using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.TeamMembers
{
    public class CreateTeamMemberServiceModel : BaseServiceModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
