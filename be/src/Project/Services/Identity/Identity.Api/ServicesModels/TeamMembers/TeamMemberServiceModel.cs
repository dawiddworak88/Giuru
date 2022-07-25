using System;

namespace Identity.Api.ServicesModels.TeamMembers
{
    public class TeamMemberServiceModel
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
