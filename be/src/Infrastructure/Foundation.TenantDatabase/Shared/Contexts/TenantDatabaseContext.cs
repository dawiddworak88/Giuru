using Foundation.TenantDatabase.Areas.Accounts.Entities;
using Foundation.TenantDatabase.Areas.Clients.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foundation.TenantDatabase.Shared.Contexts
{
    public class TenantDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public TenantDatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Accounts { get; set; }

        public DbSet<Client> Clients { get; set; }
    }
}
