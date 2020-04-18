using Foundation.GenericRepository.Entities;

namespace Foundation.Database.Shared.Repositories
{
    public interface IGenericRepository<TEntity> : Foundation.GenericRepository.Repositories.IGenericRepository<TEntity> where TEntity : class, IEntity
    {
    }
}
