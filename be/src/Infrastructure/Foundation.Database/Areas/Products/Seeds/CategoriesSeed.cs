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
            if (!context.Categories.Any(x => x.Id == Constants.CategoryGuids.Furniture))
            {
                var furniture = new Category
                {
                    Id = Constants.CategoryGuids.Furniture,
                    Level = 0
                };

                var enTranslation = new Translation
                { 
                    Key = Constants.CategoryGuids.Furniture.ToString(),
                    Language = LanguageConstants.English,
                    Value = "Furniture"
                };

                var plTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.ToString(),
                    Language = LanguageConstants.Polish,
                    Value = "Meble"
                };

                var deTranslation = new Translation
                {
                    Key = Constants.CategoryGuids.Furniture.ToString(),
                    Language = LanguageConstants.German,
                    Value = "Möbel"
                };

                context.Categories.Add(EntitySeedHelper.SeedEntity(furniture));
                context.Translations.Add(enTranslation);
                context.Translations.Add(plTranslation);
                context.Translations.Add(deTranslation);

                context.SaveChanges();
            }

            
        }
    }
}
