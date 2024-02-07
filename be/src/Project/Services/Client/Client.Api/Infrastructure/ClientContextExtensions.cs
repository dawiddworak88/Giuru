using Client.Api.Infrastructure.Notifications.Seeds;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace Client.Api.Infrastructure
{
    public static class ClientContextExtensions
    {
        public static bool AllMigrationsApplied(this ClientContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this ClientContext context)
        {
            NotificationTypeSeeds.SeedNotificationTypes(context);
        }
    }
}
