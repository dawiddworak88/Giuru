using Foundation.GenericRepository.Entities;
using Foundation.GenericRepository.Repositories;

namespace Foundation.TenantDatabase.Shared.Repositories
{
    public interface ITenantGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class, IEntity
    {
    }
}
