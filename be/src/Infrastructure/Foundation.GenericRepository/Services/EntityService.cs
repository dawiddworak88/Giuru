using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.GenericRepository.Services
{
    public class EntityService : IEntityService
    {
        public T EnrichEntity<T>(T entity, string username) where T : Entity
        {
            entity.LastModifiedBy = username;
            entity.LastModifiedDate = DateTime.UtcNow;

            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.UtcNow;

            entity.IsActive = true;

            return entity;
        }
    }
}
