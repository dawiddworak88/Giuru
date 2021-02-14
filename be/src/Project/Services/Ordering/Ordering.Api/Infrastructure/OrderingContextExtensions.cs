using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Ordering.Api.Infrastructure.Orders.Seeds;
using System.Linq;

namespace Ordering.Api.Infrastructure
{
    public static class OrderingContextExtensions
    {
        public static bool AllMigrationsApplied(this OrderingContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this OrderingContext context, IConfiguration configuration)
        {
            OrderStatesSeed.SeedOrderStates(context);
            OrderStatusesSeed.SeedOrderStatuses(context, configuration);
        }
    }
}
