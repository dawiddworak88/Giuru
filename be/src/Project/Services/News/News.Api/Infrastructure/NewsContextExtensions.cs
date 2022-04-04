using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using News.Api.Infrastructure.Seeds;
using System.Linq;

namespace News.Api.Infrastructure
{
    public static class NewsContextExtensions
    {
        public static bool AllMigrationsApplied(this NewsContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this NewsContext context)
        {
            CategoriesSeed.SeedCategories(context);
        }
    }
}
