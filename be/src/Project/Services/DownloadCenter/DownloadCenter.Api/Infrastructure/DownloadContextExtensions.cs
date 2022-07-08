using DownloadCenter.Api.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace DownloadCenter.Api.Infrastructure
{
    public static class DownloadContextExtensions
    {
        public static bool AllMigrationsApplied(this DownloadContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this DownloadContext context)
        {
            CategoriesSeed.SeedCategories(context);
            DownloadsSeed.SeedDownloads(context);
        }
    }
}
