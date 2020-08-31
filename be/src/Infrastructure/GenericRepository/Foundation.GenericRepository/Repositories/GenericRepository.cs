using Foundation.GenericRepository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.GenericRepository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbContext context;

        public GenericRepository(DbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(Guid id)
        {
            var entityToDelete = context.Set<TEntity>().FirstOrDefault(e => e.Id == id);

            if (entityToDelete != null)
            {
                entityToDelete.IsActive = false;
            }
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return context.Set<TEntity>().Where(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>();
        }

        public TEntity GetById(Guid id)
        {
            return context.Set<TEntity>().FirstOrDefault(e => e.Id == id && e.IsActive);
        }

        public void Edit(TEntity entity)
        {
            #pragma warning disable S1854 // Unused assignments should be removed
            var editedEntity = context.Set<TEntity>().FirstOrDefault(e => e.Id == entity.Id);
            editedEntity = entity;
            #pragma warning restore S1854 // Unused assignments should be removed
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
