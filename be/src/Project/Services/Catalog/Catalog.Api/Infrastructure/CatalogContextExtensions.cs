using Catalog.Api.Infrastructure.Categories.Seeds;
using Catalog.Api.Infrastructure.Products.Seeds;
using Foundation.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Catalog.Api.Infrastructure
{
    public static class CatalogContextExtensions
    {
        public static bool AllMigrationsApplied(this CatalogContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this CatalogContext context, IConfiguration configuration)
        {
            BrandsSeed.SeedBrands(context, configuration);
            CategoriesSeed.SeedCategories(context);
            CategoryImagesSeed.SeedCategoryImages(context);
        }
    }
}
