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
            SeedCategoryImage(context, CategoryConstants.CategoryGuids.Furniture.LivingRoom.SectionalsId, CategoryImageConstants.SectionalsImageMediaId);
        }

        private static void SeedCategoryImage(CatalogContext context, Guid categoryId, Guid imageId)
        {
            if (!context.CategoryImages.Any(x => x.CategoryId == categoryId))
            {
                var categoryImage = new CategoryImage
                {
                    CategoryId = CategoryConstants.CategoryGuids.Furniture.LivingRoom.SectionalsId,
                    MediaId = CategoryImageConstants.SectionalsImageMediaId,
                    Order = 0
                };

                context.CategoryImages.Add(categoryImage);

                context.SaveChanges();
            }
        }
    }
}
