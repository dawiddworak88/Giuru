using Foundation.Database.Areas.Accounts.Seeds;
using Foundation.Database.Areas.Tenants.Seeds;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace Foundation.Database.Shared.Contexts
{
    public static class DatabaseContextExtension
    {
        public static bool AllMigrationsApplied(this DatabaseContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this DatabaseContext context)
        {
            AccountsSeed.SeedAccounts(context);
            TenantsSeed.SeedTenants(context);
        }
    }
}
