using Foundation.Database.Areas.Tenants.Entities;
using Microsoft.AspNetCore.Identity;

namespace Foundation.Database.Areas.Accounts.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
