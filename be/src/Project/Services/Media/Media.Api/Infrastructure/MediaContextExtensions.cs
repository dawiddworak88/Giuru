using Media.Api.Infrastructure.Media.Seeds;
using Media.Api.Services.Checksums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Media.Api.Infrastructure
{
    public static class MediaContextExtensions
    {
        public static bool AllMigrationsApplied(this MediaContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this MediaContext context, IConfiguration configuration, IChecksumService checksumService)
        {
            MediaSeed.SeedHeaders(context, configuration["StorageConnectionString"], Guid.Parse(configuration["OrganisationId"]), checksumService);
            MediaSeed.SeedCategories(context, configuration["StorageConnectionString"], Guid.Parse(configuration["OrganisationId"]), checksumService);
            MediaSeed.SeedHeroSliderItems(context, configuration["StorageConnectionString"], Guid.Parse(configuration["OrganisationId"]), checksumService);
        }
    }
}
