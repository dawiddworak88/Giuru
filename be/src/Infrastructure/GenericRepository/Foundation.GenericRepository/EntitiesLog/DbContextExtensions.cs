using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.GenericRepository.EntitiesLog
{
    public static class DbContextExtensions
    {
        public static IEnumerable<EntityLogProperty> GetModifiedProperties(this DbContext context, string entityTypeName)
        {
            var properties = new List<EntityLogProperty>();

            context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified && x.Entity.GetType().Name == entityTypeName).ToList().ForEach(entry => {

                foreach (var property in entry.Properties)
                {
                    if (!property.IsModified)
                    {
                        continue;
                    }

                    var propertyEntry = new EntityLogProperty
                    {
                        PropertyName = property.Metadata.Name,
                        OldValue = property.OriginalValue.ToString(),
                        NewValue = property.CurrentValue.ToString()
                    };

                    properties.Add(propertyEntry);
                }
            });

            return properties;
        }
    }
}
