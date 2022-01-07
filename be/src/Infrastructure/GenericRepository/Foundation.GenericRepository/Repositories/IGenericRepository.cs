using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.GenericRepository.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity: Entity
    {
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        TEntity GetById(Guid id);

        Task CreateAsync(TEntity entity);

        void Edit(TEntity entity);

        void Delete(Guid id);

        Task SaveChangesAsync();
    }
}
