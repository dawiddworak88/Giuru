using DownloadCenter.Api.Definitions;
using DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;

namespace DownloadCenter.Api.Infrastructure.Seeds
{
    public static class CategoriesSeed
    {
        public static void SeedCategories(DownloadCenterContext context, Guid sellerId)
        {
            SeedCategory(context, DownloadCenterConstants.Categories.CollectionsCategory, null, sellerId, true, "Collections", "Kolekcje", "Sammlungen");
            SeedCategory(context, DownloadCenterConstants.Categories.ColorsOfProductsCategory, DownloadCenterConstants.Categories.CollectionsCategory, sellerId, true, "Colors of products", "Kolorystyka produktów", "Farben von Produkten");
            SeedCategory(context, DownloadCenterConstants.Categories.MarketingCategory, null, sellerId, true, "Marketing", "Marketing", "Marketing");
            SeedCategory(context, DownloadCenterConstants.Categories.BannersCategory, DownloadCenterConstants.Categories.MarketingCategory, sellerId, true, "Banners", "Banery", "Banner");
            SeedCategory(context, DownloadCenterConstants.Categories.GifsCategory, DownloadCenterConstants.Categories.MarketingCategory, sellerId, true, "Gifs", "Gify", "Gifs");
            SeedCategory(context, DownloadCenterConstants.Categories.TechnicalCategory, null, sellerId, true, "Technical data", "Dane techniczne", "Technische daten");
            SeedCategory(context, DownloadCenterConstants.Categories.TechnicalSheetsCategory, DownloadCenterConstants.Categories.TechnicalCategory, sellerId, true, "Technical cards", "Karty Techniczne", "Technische Karten");
        }

        private static void SeedCategory(DownloadCenterContext context, Guid id, Guid? parentCategoryId, Guid sellerId, bool isVisible, string englishName, string polishName, string germanName)
        {
            if (!context.DownloadCenterCategories.Any(x => x.Id == id))
            {
                var category = new DownloadCenterCategory
                {
                    Id = id,
                    ParentCategoryId = parentCategoryId,
                    IsVisible = isVisible,
                    Order = 0,
                    SellerId = sellerId
                };

                var enCategoryTranslation = new DownloadCenterCategoryTranslation
                {
                    CategoryId = category.Id,
                    Language = "en",
                    Name = englishName
                };

                var plCategoryTranslation = new DownloadCenterCategoryTranslation
                {
                    CategoryId = category.Id,
                    Language = "pl",
                    Name = polishName
                };

                var deCategoryTranslation = new DownloadCenterCategoryTranslation
                {
                    CategoryId = category.Id,
                    Language = "de",
                    Name = germanName
                };

                context.DownloadCenterCategories.Add(category.FillCommonProperties());
                context.DownloadCenterCategoryTranslations.Add(enCategoryTranslation.FillCommonProperties());
                context.DownloadCenterCategoryTranslations.Add(plCategoryTranslation.FillCommonProperties());
                context.DownloadCenterCategoryTranslations.Add(deCategoryTranslation.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
