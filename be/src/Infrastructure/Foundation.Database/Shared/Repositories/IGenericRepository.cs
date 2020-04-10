using Foundation.GenericRepository;

namespace Foundation.Database.Shared.Repositories
{
    public interface IGenericRepository<TEntity> : Foundation.GenericRepository.IGenericRepository<TEntity> where TEntity : class, IEntity
    {
    }
}
