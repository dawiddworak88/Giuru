using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Infrastructure.Accounts.Seeds;
using Identity.Api.Infrastructure.Organisations.Seeds;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Identity.Api.Infrastructure
{
    public static class IdentityContextExtension
    {
        public static bool AllMigrationsApplied(this IdentityContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this IdentityContext context, IConfiguration configuration, IUserService userService)
        {
            OrganisationsSeed.SeedOrganisations(context, configuration);
            AccountsSeed.SeedAccounts(context, configuration, userService);
        }
    }
}
