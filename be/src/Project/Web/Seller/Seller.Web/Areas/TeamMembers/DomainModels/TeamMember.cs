using System;

namespace Seller.Web.Areas.TeamMembers.DomainModels
{
    public class TeamMember
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
