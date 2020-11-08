using Catalog.Api.Infrastructure.Categories.Definitions;
using Catalog.Api.Infrastructure.Categories.Entites;
using System;
using System.Linq;

namespace Catalog.Api.Infrastructure.Categories.Seeds
{
    public static class CategoryImagesSeed
    {
        public static void SeedCategoryImages(CatalogContext context)
        {
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SofasId, CategoryImageConstants.SofasImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SectionalsId, CategoryImageConstants.SectionalsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.CoffeeTablesId, CategoryImageConstants.CoffeeTablesImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.TvStandsId, CategoryImageConstants.TvStandsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.WallUnitsId, CategoryImageConstants.WallUnitsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.ChairsId, CategoryImageConstants.ChairsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.PoufsId, CategoryImageConstants.PoufsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.LivingRoomSetsId, CategoryImageConstants.LivingRoomSetsMediaId);

            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.BedsId, CategoryImageConstants.BedsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.WardrobesId, CategoryImageConstants.WardrobesImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.Bedroom.ChestsId, CategoryImageConstants.ChestsImageMediaId);

            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.KitchenDiningRoom.DiningTablesSeatingId, CategoryImageConstants.DiningTablesSeatingImageMediaId);

            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.Bathroom.BathroomVanitiesCabinetsId, CategoryImageConstants.BathroomVanitiesCabinetsImageMediaId);

            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.KidsFurniture.KidsBedsId, CategoryImageConstants.KidsBedsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.KidsFurniture.KidsBunkBedsId, CategoryImageConstants.KidsBunkBedsImageMediaId);
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.KidsFurniture.KidsDesksId, CategoryImageConstants.KidsDesksImageMediaId);
        }

        private static void SeedCategoryImage(CatalogContext context, Guid categoryId, Guid mediaId)
        {
            if (!context.CategoryImages.Any(x => x.CategoryId == categoryId))
            {
                var categoryImage = new CategoryImage
                {
                    CategoryId = categoryId,
                    MediaId = mediaId,
                    Order = 0
                };

                context.CategoryImages.Add(categoryImage);

                context.SaveChanges();
            }
        }
    }
}
