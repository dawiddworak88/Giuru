using Global.Api.Infrastructure.Entities.Seeds;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Global.Api.Infrastructure
{
    public static class GlobalContextExtensions
    {
        public static bool AllMigrationsApplied(this GlobalContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this GlobalContext context, IConfiguration configuration)
        {
            CountriesSeed.SeedCountries(context);
        }
    }
}
