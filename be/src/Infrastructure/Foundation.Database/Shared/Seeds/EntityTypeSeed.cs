using Foundation.Database.Areas.Translations.Definitions.Languages;
using Foundation.Database.Areas.Translations.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Definitions.EntityTypes;
using Foundation.Database.Shared.Entities;
using Foundation.Database.Shared.Helpers;
using System.Linq;

namespace Foundation.Database.Shared.Seeds
{
    public static class EntityTypeSeed
    {
        public static void EnsureEntityTypesSeeded(DatabaseContext context)
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

            if (!context.EntityTypes.Any(x => x.Name == EntityTypeConstants.Order))
            {
                var orderEntityType = EntitySeedHelper.SeedEntity(new EntityType { Id = EntityTypeConstants.OrderId, Name = EntityTypeConstants.Order });

                context.EntityTypes.Add(orderEntityType);

                var enTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = orderEntityType.Id.ToString(), Language = LanguageConstants.English, Value = "Order" });
                var deTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = orderEntityType.Id.ToString(), Language = LanguageConstants.German, Value = "Bestellung" });
                var plTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = orderEntityType.Id.ToString(), Language = LanguageConstants.Polish, Value = "Zamówienie" });

                context.Translations.Add(enTranslationProductEntityType);
                context.Translations.Add(deTranslationProductEntityType);
                context.Translations.Add(plTranslationProductEntityType);

                context.SaveChanges();
            }

            if (!context.EntityTypes.Any(x => x.Name == EntityTypeConstants.OrderItem))
            {
                var orderItemEntityType = EntitySeedHelper.SeedEntity(new EntityType { Id = EntityTypeConstants.OrderItemId, Name = EntityTypeConstants.OrderItem });

                context.EntityTypes.Add(orderItemEntityType);

                var enTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = orderItemEntityType.Id.ToString(), Language = LanguageConstants.English, Value = "Order item" });
                var deTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = orderItemEntityType.Id.ToString(), Language = LanguageConstants.German, Value = "Bestellungsartikel" });
                var plTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = orderItemEntityType.Id.ToString(), Language = LanguageConstants.Polish, Value = "Pozycja zamówienia" });

                context.Translations.Add(enTranslationProductEntityType);
                context.Translations.Add(deTranslationProductEntityType);
                context.Translations.Add(plTranslationProductEntityType);

                context.SaveChanges();
            }

            if (!context.EntityTypes.Any(x => x.Name == EntityTypeConstants.Client))
            {
                var clientEntityType = EntitySeedHelper.SeedEntity(new EntityType { Id = EntityTypeConstants.ClientId, Name = EntityTypeConstants.Client });

                context.EntityTypes.Add(clientEntityType);

                var enTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = clientEntityType.Id.ToString(), Language = LanguageConstants.English, Value = "Client" });
                var deTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = clientEntityType.Id.ToString(), Language = LanguageConstants.German, Value = "Kunde" });
                var plTranslationProductEntityType = EntitySeedHelper.SeedEntity(new Translation { Key = clientEntityType.Id.ToString(), Language = LanguageConstants.Polish, Value = "Klient" });

                context.Translations.Add(enTranslationProductEntityType);
                context.Translations.Add(deTranslationProductEntityType);
                context.Translations.Add(plTranslationProductEntityType);

                context.SaveChanges();
            }
        }
    }
}
