using Catalog.Api.Infrastructure.Categories.Definitions;
using Catalog.Api.Infrastructure.Categories.Entities;
using Foundation.GenericRepository.Helpers;
using System;
using System.Linq;
using Catalog.Api.Infrastructure.Categories.Entites;

namespace Catalog.Api.Infrastructure.Categories.Seeds
{
    public static class CategoriesSeed
    {
        public static void SeedCategories(CatalogContext context)
        {
            // Level 0
            SeedCategory(context, CategoryConstants.CategoryGuids.FurnitureId, 0, null, 0, false, "Furniture", "Meble", "Möbel");

            // Level 1 Furniture
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 0, false, "Living Room Furniture", "Meble do salonu", "Wohnzimmermöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.BedroomFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 10, false, "Bedroom Furniture", "Meble do sypialni", "Schlafzimmermöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.AccentFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 30, false, "Accent Furniture", "Meble dekoracyjne", "Kleinmöbel");

            // Level 2 Living Room
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SofasId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 0, true, "Sofas & Loveseats", "Sofy i Kanapy", "Sofas & Couches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SectionalsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 10, true, "Sectionals", "Narożniki", "Ecksofas & Eckcouches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.CoffeeTablesId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 20, true, "Coffee Tables", "Stoliki kawowe i ławy", "Wohnzimmertische");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.ChairsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 50, true, "Chairs & Recliners", "Krzesła i fotele", "Sessel & Stühle");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.PoufsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 60, true, "Poufs", "Pufy", "Fußhocker");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.LivingRoomSetsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 70, true, "Lounge suites", "Zestawy", "Sitzgruppen");

            // Level 2 Bedroom
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.BedsId, 2, CategoryConstants.CategoryGuids.Furniture.BedroomFurnitureId, 0, true, "Beds & Box Springs", "Łóżka & Box Spring", "Betten & Box Springs");
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

                context.Categories.Add(EntityHelper.SeedEntity(category));
                context.CategoryTranslations.Add(EntityHelper.SeedEntity(enCategoryTranslation));
                context.CategoryTranslations.Add(EntityHelper.SeedEntity(plCategoryTranslation));
                context.CategoryTranslations.Add(EntityHelper.SeedEntity(deCategoryTranslation));

                context.SaveChanges();
            }
        }
    }
}
