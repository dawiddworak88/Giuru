using Foundation.Localization.Definitions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Basket.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Foundation.EventBus.Abstractions;
using Basket.Api.IntegrationEvents;
using Basket.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Basket.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.Configure<LocalizationSettings>(configuration);
        }

        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderStartedIntegrationEvent, IIntegrationEventHandler<OrderStartedIntegrationEvent>>();
        }

        public static void ConfigureDatabaseMigrations(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<BasketContext>();

            if (!dbContext.AllMigrationsApplied())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
