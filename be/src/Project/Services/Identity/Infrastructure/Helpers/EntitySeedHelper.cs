using Foundation.GenericRepository.Entities;
using System;

namespace Identity.Api.Infrastructure.Helpers
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
