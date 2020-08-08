using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Infrastructure.Accounts.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
