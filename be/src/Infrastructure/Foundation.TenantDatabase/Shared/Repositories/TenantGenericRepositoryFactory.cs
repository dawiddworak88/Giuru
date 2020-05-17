using Foundation.GenericRepository.Entities;
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

        public virtual async Task<ITenantGenericRepository<TEntity>> CreateTenantGenericRepository<TEntity>(string connectionString) where TEntity : class, IEntity
        {
            var context = await this.tenantDatabaseContextFactory.CreateDbContextAsync(connectionString);

            return new TenantGenericRepository<TEntity>(context);
        }
    }
}
