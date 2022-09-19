using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Catalog.BackgroundTasks.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static IServiceCollection ConigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy());

            if (string.IsNullOrWhiteSpace(configuration["ConnectionString"]) is false)
            {
                hcBuilder.AddSqlServer(
                    configuration["ConnectionString"],
                    name: "catalog-backgroundtasks-db",
                    tags: new string[] { "catalogapidb" });
            }

            if (string.IsNullOrWhiteSpace(configuration["ElasticsearchUrl"]) is false)
            {
                hcBuilder
                    .AddElasticsearch(
                        configuration["ElasticsearchUrl"],
                        name: "catalog-backgroundtasks-search",
                        tags: new string[] { "catalogapisearch" }
                    );
            }

            if (string.IsNullOrWhiteSpace(configuration["EventBusConnection"]) is false)
            {
                hcBuilder
                    .AddRabbitMQ(
                        configuration["EventBusConnection"],
                        name: "catalog-backgroundtasks-messagebus",
                        tags: new string[] { "messagebus" });
            }

            return services;
        }
    }
}
