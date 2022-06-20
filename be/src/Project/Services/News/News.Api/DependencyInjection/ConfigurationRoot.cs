using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using News.Api.Infrastructure;

namespace News.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<NewsContext>();

                if (!dbContext.AllMigrationsApplied())
                {
                    dbContext.Database.Migrate();
                    dbContext.EnsureSeeded();
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
                    name: "news-api-db",
                    tags: new string[] { "newsapidb" });
            }

            return services;
        }
    }
}
