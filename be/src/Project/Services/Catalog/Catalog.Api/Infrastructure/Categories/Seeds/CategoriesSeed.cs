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
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.KichtenDiningFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 20, false, "Kitchen & Dining Furniture", "Meble do kuchni i jadalni", "Esszimmer- & Küchenmöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.AccentFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 30, false, "Accent Furniture", "Meble dekoracyjne", "Kleinmöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.OfficeFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 40, false, "Office Furniture", "Meble do biura", "Büromöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.EntryFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 50, false, "Entry & Mudroom Furniture", "Meble do przedpokoju", "Flur- & Dielenmöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.OutdoorFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 60, false, "Outdoor & Patio Furniture", "Meble do ogrodu", "Gartenmöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.BathroomFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 70, false, "Bathroom Furniture", "Meble łazienkowe", "Badmöbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.KidsFurnitureId, 1, CategoryConstants.CategoryGuids.FurnitureId, 80, false, "Baby & Kids Furniture", "Meble do pokoju dziecka", "Kinderzimmermöbel");

            // Level 2 Living Room
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SofasId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 0, true, "Sofas & Loveseats", "Sofy i Kanapy", "Sofas & Couches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SectionalsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 10, true, "Sectionals", "Narożniki", "Ecksofas & Eckcouches");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.CoffeeTablesId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 20, true, "Coffee Tables", "Stoliki kawowe i ławy", "Wohnzimmertische");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.TvStandsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 30, true, "TV Stands", "Panele i szafki RTV", "TV-Möbel");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.WallUnitsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 40, true, "Wall Units", "Meblościanki", "Wohnwände");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.ChairsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 50, true, "Chairs & Recliners", "Krzesła i fotele", "Sessel & Stühle");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.PoufsId, 2, CategoryConstants.CategoryGuids.Furniture.LivingRoomFurnitureId, 60, true, "Poufs", "Pufy", "Fußhocker");

            // Level 2 Bedroom
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.BedsId, 2, CategoryConstants.CategoryGuids.Furniture.BedroomFurnitureId, 0, true, "Beds", "Łóżka", "Betten");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.WardrobesId, 2, CategoryConstants.CategoryGuids.Furniture.BedroomFurnitureId, 10, true, "Armoires & Wardrobes", "Szafy", "Kleiderschränke");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.ChestsId, 2, CategoryConstants.CategoryGuids.Furniture.BedroomFurnitureId, 20, true, "Dressers & Chests", "Komody", "Kommoden & Sideboards");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.MattressesId, 2, CategoryConstants.CategoryGuids.Furniture.BedroomFurnitureId, 30, true, "Mattresses & Box Springs", "Materace & Box Spring", "Matratzen & Box Springs");

            // Level 2 Kitchen and Dining Room
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.KitchenDiningRoom.DiningTablesSeatingId, 2, CategoryConstants.CategoryGuids.Furniture.KichtenDiningFurnitureId, 0, true, "Dining Tables & Seating", "Stoły i krzesła kuchenne", "Esstische & Esszimmerstühle");

            // Level 2 Bathroom
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.Bathroom.BathroomVanitiesCabinetsId, 2, CategoryConstants.CategoryGuids.Furniture.BathroomFurnitureId, 0, true, "Bathroom Vanities & Cabinets", "Szafki i komody łazienkowe", "Badezimmerschränke & Badregale");

            // Level 2 Kids Room
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.KidsFurniture.KidsBedsId, 2, CategoryConstants.CategoryGuids.Furniture.KidsFurnitureId, 0, true, "Children Beds", "Łóżka dziecięce", "Kinderbetten");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.KidsFurniture.KidsBunkBedsId, 2, CategoryConstants.CategoryGuids.Furniture.KidsFurnitureId, 10, true, "Children Bunk Beds", "Łóżka piętrowe dla dzieci", "Hoch- & Etagenbetten");
            SeedCategory(context, CategoryConstants.CategoryGuids.Furniture.KidsFurniture.KidsDesksId, 2, CategoryConstants.CategoryGuids.Furniture.KidsFurnitureId, 40, true, "Children Desks", "Biurka dziecięce", "Kinderschreibtische");
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

                context.Categories.Add(EntitySeedHelper.SeedEntity(category));
                context.CategoryTranslations.Add(EntitySeedHelper.SeedEntity(enCategoryTranslation));
                context.CategoryTranslations.Add(EntitySeedHelper.SeedEntity(plCategoryTranslation));
                context.CategoryTranslations.Add(EntitySeedHelper.SeedEntity(deCategoryTranslation));

                context.SaveChanges();
            }
        }
    }
}
