using Microsoft.AspNetCore.Identity;

namespace Foundation.TenantDatabase.Areas.Accounts.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
