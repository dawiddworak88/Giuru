using Foundation.Database.Areas.Accounts.Seeds;
using Foundation.Database.Areas.Tenants.Seeds;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
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

        public static void EnsureSeeded(this DatabaseContext context, IConfiguration configuration)
        {
            AccountsSeed.SeedAdminAccounts(context, configuration);
            TenantsSeed.SeedTenants(context, configuration);
            AccountsSeed.SeedTenantAccounts(context, configuration);
        }
    }
}
