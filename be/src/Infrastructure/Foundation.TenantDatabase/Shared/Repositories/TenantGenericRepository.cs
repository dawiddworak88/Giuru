using Foundation.GenericRepository.Entities;
using Foundation.GenericRepository.Repositories;
using Foundation.TenantDatabase.Shared.Contexts;

namespace Foundation.TenantDatabase.Shared.Repositories
{
    public class TenantGenericRepository<TEntity> : GenericRepository<TEntity>, ITenantGenericRepository<TEntity> where TEntity: class, IEntity
    {
        public TenantGenericRepository(TenantDatabaseContext context) : base(context)
        {
        }
    }
}
