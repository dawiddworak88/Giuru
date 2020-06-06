using Foundation.GenericRepository.Entities;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.GenericRepository.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity: class, IEntity
    {
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        PagedResults<IEnumerable<TEntity>> GetPaged(int pageIndex, int itemsPerPage, Func<TEntity, bool> predicate, Func<TEntity, object> orderByPredicate = null, bool? isOrderByDescending = null);

        TEntity GetById(Guid id);

        Task CreateAsync(TEntity entity);

        void Edit(TEntity entity);

        void Delete(Guid id);

        Task SaveChangesAsync();
    }
}
