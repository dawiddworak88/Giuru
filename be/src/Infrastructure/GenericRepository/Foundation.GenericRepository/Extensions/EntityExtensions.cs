using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.GenericRepository.Extensions
{
    public static class EntityExtensions
    {
        public static T FillCommonProperties<T>(this T entity) where T : Entity
        {
            entity.IsActive = true;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;

            return entity;
        }
    }
}
