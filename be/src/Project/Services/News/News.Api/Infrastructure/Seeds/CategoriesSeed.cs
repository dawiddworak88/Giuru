using Foundation.GenericRepository.Extensions;
using News.Api.Definitions;
using News.Api.Infrastructure.Entities;
using System;
using System.Linq;

namespace News.Api.Infrastructure.Seeds
{
    public static class CategoriesSeed
    {

        public static void SeedCategories(NewsContext context)
        {
            SeedCategory(context, NewsConstants.Categories.InformationCategory, null, "Information", "Informacje", "Information");
        }

        private static void SeedCategory(NewsContext context, Guid id, Guid? parentCategoryId, string englishName, string polishName, string germanName)
        {
            if (!context.Categories.Any(x => x.Id == id))
            {
                var category = new Categories
                {
                    Id = id,
                    ParentCategoryId = parentCategoryId,
                };

                var enCategoryTranslation = new CategoryTranslation
                {
                    CategoryId = category.Id,
                    Language = "en",
                    Name = englishName
                };

                var plCategoryTranslation = new CategoryTranslation
                {
                    CategoryId = category.Id,
                    Language = "pl",
                    Name = polishName
                };

                var deCategoryTranslation = new CategoryTranslation
                {
                    CategoryId = category.Id,
                    Language = "de",
                    Name = germanName
                };

                context.Categories.Add(category.FillCommonProperties());
                context.CategoryTranslations.Add(enCategoryTranslation.FillCommonProperties());
                context.CategoryTranslations.Add(plCategoryTranslation.FillCommonProperties());
                context.CategoryTranslations.Add(deCategoryTranslation.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
