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
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SofasId, 1, CategoryConstants.CategoryGuids.FurnitureId, 0, true, "Sofas & Loveseats", "Sofy i Kanapy", "Sofas & Couches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SectionalsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 10, true, "Sectionals", "Narożniki", "Ecksofas & Eckcouches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.CoffeeTablesId, 1, CategoryConstants.CategoryGuids.FurnitureId, 20, true, "Coffee Tables", "Stoliki kawowe i ławy", "Wohnzimmertische");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.ChairsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 50, true, "Chairs & Recliners", "Krzesła i fotele", "Sessel & Stühle");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.PoufsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 60, true, "Poufs", "Pufy", "Fußhocker");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.LivingRoomSetsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 70, true, "Lounge suites", "Zestawy", "Sitzgruppen");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.BedsId, 1, CategoryConstants.CategoryGuids.FurnitureId, 80, true, "Beds & Box Springs", "Łóżka & Box Spring", "Betten & Box Springs");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.MattressesId, 1, CategoryConstants.CategoryGuids.FurnitureId, 90, true, "Mattresses", "Materace", "Matratzen");
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
