using Foundation.TenantDatabase.Areas.Translations.Definitions.Languages;
using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Contexts;
using Foundation.TenantDatabase.Shared.Definitions.EntityTypes;
using Foundation.TenantDatabase.Shared.Entities;
using Foundation.TenantDatabase.Shared.Helpers;
using System.Linq;

namespace Foundation.TenantDatabase.Shared.Seeds
{
    public static class EntityTypeSeed
    {
        public static void EnsureEntityTypesSeeded(this TenantDatabaseContext context)
        {
            // Adds entity types
            if (!context.EntityTypes.Any(x => x.Name == EntityTypeConstants.Product))
            {
                var productItemEntityType = EntitySeedHelper.SeedEntity(new EntityType { Id = EntityTypeConstants.ProductId, Name = EntityTypeConstants.Product });

                context.EntityTypes.Add(productItemEntityType);

                var enTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = productItemEntityType.Id.ToString(), Language = LanguageConstants.English, Value = "Product" });
                var deTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = productItemEntityType.Id.ToString(), Language = LanguageConstants.German, Value = "Produkt" });
                var plTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = productItemEntityType.Id.ToString(), Language = LanguageConstants.Polish, Value = "Produkt" });

                context.Translations.Add(enTranslationProductEntityType);
                context.Translations.Add(deTranslationProductEntityType);
                context.Translations.Add(plTranslationProductEntityType);

                context.SaveChanges();
            }
        }
    }
}
