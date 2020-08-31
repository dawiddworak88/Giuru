using Microsoft.AspNetCore.Identity;
using System;

namespace Identity.Api.Infrastructure.Accounts.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
