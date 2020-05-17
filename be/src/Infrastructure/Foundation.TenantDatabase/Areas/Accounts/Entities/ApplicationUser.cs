using Microsoft.AspNetCore.Identity;
using System;

namespace Foundation.TenantDatabase.Areas.Accounts.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Guid ClientId { get; set; }
    }
}
