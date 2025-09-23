using Catalog.Api.Infrastructure;
using Foundation.Catalog.Infrastructure;
using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Catalog.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<CatalogContext>();

                if (!dbContext.AllMigrationsApplied())
                {
                    dbContext.Database.Migrate();
                    dbContext.EnsureSeeded(configuration);
                }
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
                    name: "catalog-api-db",
                    tags: new string[] { "catalogapidb" });
            }

            if (string.IsNullOrWhiteSpace(configuration["ElasticsearchUrl"]) is false)
            {
                hcBuilder
                    .AddElasticsearch(
                        configuration["ElasticsearchUrl"],
                        name: "catalog-api-search",
                        tags: new string[] { "catalogapisearch" }
                    );
            }

            if (string.IsNullOrWhiteSpace(configuration["EventBusConnection"]) is false)
            {
                hcBuilder
                    .AddRabbitMQ(
                        configuration["EventBusConnection"],
                        name: "catalog-api-messagebus",
                        tags: new string[] { "messagebus" });
            }

            return services;
        }
    }
}
