﻿using Analytics.Api.Configurations;
using Analytics.Api.Infrastructure;
using Analytics.Api.IntegrationEvents;
using Foundation.EventBus.Abstractions;
using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Analytics.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AnalyticsContext>();

                if (!dbContext.AllMigrationsApplied())
                {
                    dbContext.Database.Migrate();
                }
            }
        }

        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalizationSettings>(configuration);
            services.Configure<AppSettings>(configuration);
        }

        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetService<IEventBus>();

            eventBus.Subscribe<OrderStartedIntegrationEvent, IIntegrationEventHandler<OrderStartedIntegrationEvent>>();
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
                    name: "analytics-api-db",
                    tags: new string[] { "analyticsapidb" });
            }

            return services;
        }
    }
}

