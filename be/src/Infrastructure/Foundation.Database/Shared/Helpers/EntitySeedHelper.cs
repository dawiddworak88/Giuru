using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Database.Shared.Helpers
{
    public static class EntitySeedHelper
    {
        public static T SeedEntity<T>(T entity) where T: Entity
        {
            entity.IsActive = true;
            entity.LastModifiedBy = "system";
            entity.CreatedBy = "system";
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;

            return entity;
        }
    }
}
