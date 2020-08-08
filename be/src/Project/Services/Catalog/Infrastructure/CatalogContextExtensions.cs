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
            //AccountsSeed.SeedAdminAccounts(context, configuration);
            //SellersSeed.SeedSellers(context, configuration);
            //AccountsSeed.SeedSellerAccounts(context, configuration);
        }
    }
}
