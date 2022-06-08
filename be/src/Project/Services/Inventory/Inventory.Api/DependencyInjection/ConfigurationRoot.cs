using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Foundation.Localization.Definitions;
using Inventory.Api.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Inventory.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<InventoryContext>();

            if (!dbContext.AllMigrationsApplied())
            {
                dbContext.Database.Migrate();
            }
        }

        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalizationSettings>(configuration);
        }

        public static IServiceCollection ConigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy());

            if (string.IsNullOrWhiteSpace(configuration["ConnectionString"]) is false)
            {
                hcBuilder.AddSqlServer(
                    configuration["ConnectionString"],
                    name: "inventory-api-db",
                    tags: new string[] { "inventoryapidb" });
            }

            if (string.IsNullOrWhiteSpace(configuration["EventBusConnection"]) is false)
            {
                hcBuilder
                    .AddRabbitMQ(
                        configuration["EventBusConnection"],
                        name: "inventory-api-messagebus",
                        tags: new string[] { "messagebus" });
            }

            return services;
        }
    }
}
