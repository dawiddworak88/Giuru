using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Database.Shared.Helpers
{
    public static class EntitySeedHelper
    {
        public static T SeedEntity<T>(T entity) where T: Entity
        {
            entity.IsActive = true;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;

            return entity;
        }
    }
}
