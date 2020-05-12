using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.TenantDatabase.Shared.Contexts
{
    public class TenantDatabaseContextFactory
    {
        public virtual async Task<TenantDatabaseContext> CreateDbContextAsync(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantDatabaseContext>();

            optionsBuilder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly("Foundation.TenantDatabase")).UseLazyLoadingProxies();

            var context = new TenantDatabaseContext(optionsBuilder.Options);

            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();

                context.EnsureSeeded();
            }

            return context;
        }
    }
}
