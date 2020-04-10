using Foundation.GenericRepository;

namespace Foundation.TenantDatabase.Shared.Repositories
{
    public interface ITenantGenericRepository<TEntity> : Foundation.GenericRepository.IGenericRepository<TEntity> where TEntity: class, IEntity
    {
    }
}
