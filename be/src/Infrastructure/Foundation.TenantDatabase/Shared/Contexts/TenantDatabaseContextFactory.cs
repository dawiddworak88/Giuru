using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Foundation.TenantDatabase.Shared.Contexts
{
    public class TenantDatabaseContextFactory
    {
        public async Task<TenantDatabaseContext> CreateDbContextAsync(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantDatabaseContext>();

            optionsBuilder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly("Foundation.TenantDatabase")).UseLazyLoadingProxies();

            var context = new TenantDatabaseContext(optionsBuilder.Options);

            await context.Database.MigrateAsync();

            context.EnsureSeeded();

            return context;
        }
    }
}
