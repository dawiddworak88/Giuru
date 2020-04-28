using Foundation.TenantDatabase.Shared.Definitions.EntityTypes;
using Foundation.TenantDatabase.Shared.Definitions.Languages;
using Foundation.TenantDatabase.Shared.Entities;
using Foundation.TenantDatabase.Shared.Helpers;
using System.Linq;

namespace Foundation.TenantDatabase.Shared.Contexts
{
    public static class TenantDatabaseContextExtension
    {
        public static void EnsureSeeded(this TenantDatabaseContext context)
        {
            // Adds entity types
            if (!context.Items.Any(x => x.Id == EntityType.Product))
            {
                var productItemEntityType = EntitySeedHelper.SeedEntity(new Item { Id = EntityType.Product });

                context.Items.Add(productItemEntityType);

                var enTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Item = productItemEntityType, Language = Languages.English, Value = "Product" });
                var deTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Item = productItemEntityType, Language = Languages.German, Value = "Produkt" });
                var plTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Item = productItemEntityType, Language = Languages.Polish, Value = "Produkt" });

                context.Translations.Add(enTranslationProductEntityType);
                context.Translations.Add(deTranslationProductEntityType);
                context.Translations.Add(plTranslationProductEntityType);

                context.SaveChanges();
            }
        }
    }
}
