using Foundation.Database.Shared.Contexts;
using Foundation.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Database.Shared.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DatabaseContext context;

        public GenericRepository(DatabaseContext context)
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
                context.Set<TEntity>().Remove(entityToDelete);
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
            return context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }

        public void Edit(TEntity entity)
        {
            #pragma warning disable S1854 // Unused assignments should be removed
            var editedEntity = context.Set<TEntity>().FirstOrDefault(e => e.Id == entity.Id);
            editedEntity = entity;
            #pragma warning restore S1854 // Unused assignments should be removed
        }

        public void SaveChanges() 
        {
            context.SaveChanges();
        } 
    }
}
