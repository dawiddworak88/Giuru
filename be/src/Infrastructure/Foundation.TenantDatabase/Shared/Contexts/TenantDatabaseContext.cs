using Microsoft.EntityFrameworkCore;

namespace Foundation.TenantDatabase.Shared.Contexts
{
    public class TenantDatabaseContext : DbContext
    {
        public TenantDatabaseContext(DbContextOptions options) : base(options)
        {
        }
    }
}
