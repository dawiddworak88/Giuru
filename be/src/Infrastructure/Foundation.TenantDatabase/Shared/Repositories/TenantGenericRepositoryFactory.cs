using Foundation.TenantDatabase.Shared.Contexts;
using System.Threading.Tasks;

namespace Foundation.TenantDatabase.Shared.Repositories
{
    public class TenantGenericRepositoryFactory
    {
        private readonly TenantDatabaseContextFactory tenantDatabaseContextFactory;

        public TenantGenericRepositoryFactory(TenantDatabaseContextFactory tenantDatabaseContextFactory)
        {
            this.tenantDatabaseContextFactory = tenantDatabaseContextFactory;
        }

        public virtual async Task<TenantDatabaseContext> CreateTenantDatabaseContext(string connectionString)
        {
            return await this.tenantDatabaseContextFactory.CreateDbContextAsync(connectionString);
        }
    }
}
