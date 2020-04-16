using Foundation.Database.Shared.Contexts;
using Foundation.GenericRepository.Entities;

namespace Foundation.Database.Shared.Repositories
{
    public class GenericRepository<TEntity> : GenericRepository.Repositories.GenericRepository<TEntity>, IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        public GenericRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
