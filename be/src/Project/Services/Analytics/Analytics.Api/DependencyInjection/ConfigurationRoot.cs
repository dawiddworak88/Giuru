using Foundation.Localization.Definitions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Analytics.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalizationSettings>(configuration);
        }

        public static IServiceCollection ConigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy());

            if (string.IsNullOrWhiteSpace(configuration["EventBusConnection"]) is false)
            {
                hcBuilder
                    .AddRabbitMQ(
                        configuration["EventBusConnection"],
                        name: "basket-api-messagebus",
                        tags: new string[] { "messagebus" });
            }

            return services;
        }
    }
}
