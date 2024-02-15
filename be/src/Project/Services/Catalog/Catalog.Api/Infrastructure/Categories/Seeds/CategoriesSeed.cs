using Catalog.Api.Infrastructure.Categories.Definitions;
using System;
using System.Linq;
using Foundation.GenericRepository.Extensions;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.Categories.Entities;
using Foundation.Catalog.Infrastructure.Categories.Entites;

namespace Catalog.Api.Infrastructure.Categories.Seeds
{
    public static class CategoriesSeed
    {
        public static void SeedCategories(CatalogContext context)
        {
            // Level 0
            SeedCategory(context, CategoryConstants.CategoryGuids.FurnitureId, 0, null, 0, false, "Furniture", "Meble", "Möbel");

            // Level 1
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SofasId, 1, CategoryConstants.CategoryGuids.FurnitureId, 1, true, "Sofas & Loveseats", "Sofy i Kanapy", "Sofas & Couches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SectionalsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 2, true, "Sectionals", "Narożniki", "Ecksofas & Eckcouches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.CoffeeTablesId, 1, CategoryConstants.CategoryGuids.FurnitureId, 3, true, "Coffee Tables", "Stoliki kawowe i ławy", "Wohnzimmertische");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.ChairsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 4, true, "Chairs & Recliners", "Krzesła i fotele", "Sessel & Stühle");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.PoufsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 5, true, "Poufs", "Pufy", "Fußhocker");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.LivingRoomSetsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 6, true, "Lounge suites", "Zestawy", "Sitzgruppen");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.BedsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 7, true, "Beds & Box Springs", "Łóżka & Box Spring", "Betten & Box Springs");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.MattressesId, 1, CategoryConstants.CategoryGuids.FurnitureId, 8, true, "Mattresses", "Materace", "Matratzen");
        }

        private static void SeedCategory(CatalogContext context, Guid id, int level, Guid? parentId, int order, bool isLeaf, string englishName, string polishName, string germanName)
        {
            if (!context.Categories.Any(x => x.Id == id))
            {
                var category = new Category
                {
                    Id = id,
                    Level = level,
                    Parentid = parentId,
                    Order = order,
                    IsLeaf = isLeaf
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
