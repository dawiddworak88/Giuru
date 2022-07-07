using Download.Api.Definitions;
using Download.Api.Infrastructure.Entities.Categories;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;

namespace Download.Api.Infrastructure.Seeds
{
    public static class CategoriesSeed
    {
        public static void SeedCategories(DownloadContext context)
        {
            SeedCategory(context, DownloadConstants.Categories.CollectionsCategory, null, true, "Collections", "Kolekcje", "Sammlungen");
            SeedCategory(context, DownloadConstants.Categories.ColorsOfProductsCategory, DownloadConstants.Categories.CollectionsCategory, true, "Colors of products", "Kolorystyka produktów", "Farben von Produkten");
            SeedCategory(context, DownloadConstants.Categories.MarketingCategory, null, true, "Marketing", "Marketing", "Marketing");
            SeedCategory(context, DownloadConstants.Categories.BannersCategory, DownloadConstants.Categories.MarketingCategory, true, "Banners", "Banery", "Banner");
            SeedCategory(context, DownloadConstants.Categories.GifsCategory, DownloadConstants.Categories.MarketingCategory, true, "Gifs", "Gify", "Gifs");
            SeedCategory(context, DownloadConstants.Categories.TechnicalCategory, null, true, "Technical data", "Dane techniczne", "Technische daten");
            SeedCategory(context, DownloadConstants.Categories.TechnicalSheetsCategory, DownloadConstants.Categories.TechnicalCategory, true, "Technical cards", "Karty Techniczne", "Technische Karten");
        }

        private static void SeedCategory(DownloadContext context, Guid id, Guid? parentCategoryId, bool isVisible, string englishName, string polishName, string germanName)
        {
            if (!context.Categories.Any(x => x.Id == id))
            {
                var category = new Category
                {
                    Id = id,
                    ParentCategoryId = parentCategoryId,
                    IsVisible = isVisible,
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
