using Foundation.Database.Areas.Products.Definitions;
using Foundation.Database.Areas.Products.Entities;
using Foundation.Database.Areas.Translations.Definitions.Languages;
using Foundation.Database.Areas.Translations.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Helpers;
using System.Linq;

namespace Foundation.Database.Areas.Products.Seeds
{
    public static class CategoriesSeed
    {
        public static void SeedCategories(DatabaseContext context)
        {
            // Level 0
            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.FurnitureId))
            {
                var furniture = new Category
                {
                    Id = Constants.CategoryGuids.FurnitureId,
                    Level = 0
                };

                var enTranslation = new Translation
                { 
                    Key = Constants.CategoryGuids.FurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.FurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.FurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Möbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(furniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            // Level 1
            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.LivingRoomFurnitureId))
            {
                var livingRoom = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.LivingRoomFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 0
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.LivingRoomFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Living Room Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.LivingRoomFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble do salonu"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.LivingRoomFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Wohnzimmermöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(livingRoom));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.BedroomFurnitureId))
            {
                var bedroom = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.BedroomFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 10
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BedroomFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Bedroom Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BedroomFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble do sypialni"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BedroomFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Schlafzimmermöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(bedroom));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.KichtenDiningFurnitureId))
            {
                var kitchenDining = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.KichtenDiningFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 20
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.KichtenDiningFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Kitchen & Dining Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.KichtenDiningFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble do kuchni i jadalni"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.KichtenDiningFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Esszimmer- & Küchenmöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(kitchenDining));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.AccentFurnitureId))
            {
                var accentFurniture = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.AccentFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 30
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.AccentFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Accent Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.AccentFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble dekoracyjne"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.AccentFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Kleinmöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(accentFurniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.OfficeFurnitureId))
            {
                var officeFurniture = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.OfficeFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 40
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.OfficeFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Office Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.OfficeFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble do biura"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.OfficeFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Büromöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(officeFurniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.EntryFurnitureId))
            {
                var entryFurniture = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.EntryFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 50
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.EntryFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Entry & Mudroom Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.EntryFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble do przedpokoju"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.EntryFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Flur- & Dielenmöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(entryFurniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.OutdoorFurnitureId))
            {
                var outdoorFurniture = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.OutdoorFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 60
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.OutdoorFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Outdoor & Patio Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.OutdoorFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble do ogrodu"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.OutdoorFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Gartenmöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(outdoorFurniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.BathroomFurnitureId))
            {
                var bathroomFurniture = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.BathroomFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 70
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BathroomFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Bathroom Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BathroomFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble łazienkowe"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BathroomFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Badmöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(bathroomFurniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture.BathroomFurnitureId))
            {
                var kidsFurniture = new Category
                {
                    Id = Constants.CategoryGuids.Furniture.BathroomFurnitureId,
                    Level = 1,
                    Parentid = Constants.CategoryGuids.FurnitureId,
                    Order = 80
                };

                var enTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BathroomFurnitureId.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Baby & Kids Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BathroomFurnitureId.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble do pokoju dziecka"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.BathroomFurnitureId.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Kinderzimmermöbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(kidsFurniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }
        }
    }
}
