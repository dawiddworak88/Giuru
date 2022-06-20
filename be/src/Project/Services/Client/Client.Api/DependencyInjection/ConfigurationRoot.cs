using Client.Api.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Foundation.Localization.Definitions;
using Foundation.EventBus.Abstractions;
using Client.Api.IntegrationEvents;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Client.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ClientContext>();

                if (!dbContext.AllMigrationsApplied())
                {
                    dbContext.Database.Migrate();
                }
            }
        }

        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalizationSettings>(configuration);
        }

        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<ClientApplicationIntegrationEvent, IIntegrationEventHandler<ClientApplicationIntegrationEvent>>();
        public static IServiceCollection ConigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy());

            if (string.IsNullOrWhiteSpace(configuration["ConnectionString"]) is false)
            {
                hcBuilder.AddSqlServer(
                    configuration["ConnectionString"],
                    name: "client-api-db",
                    tags: new string[] { "clientdb" });
            }

            return services;
        }
    }
}
