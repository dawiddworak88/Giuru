using Foundation.TenantDatabase.Areas.Clients.Entities;
using Microsoft.AspNetCore.Identity;

namespace Foundation.TenantDatabase.Areas.Accounts.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Client Client { get; set; }
    }
}
