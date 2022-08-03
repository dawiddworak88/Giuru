using DownloadCenter.Api.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace DownloadCenter.Api.Infrastructure
{
    public static class DownloadCenterContextExtensions
    {
        public static bool AllMigrationsApplied(this DownloadCenterContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this DownloadCenterContext context, IConfiguration configuration)
        {
            CategoriesSeed.SeedCategories(context, Guid.Parse(configuration["OrganisationId"]));
        }
    }
}
