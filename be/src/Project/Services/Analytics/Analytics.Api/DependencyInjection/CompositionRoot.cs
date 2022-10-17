using Analytics.Api.Infrastructure;
using Analytics.Api.Services.SalesAnalytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Analytics.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAnalyticsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ISalesService, SalesService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AnalyticsContext>();

            services.AddDbContext<AnalyticsContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite().MigrationsAssembly("Analytics.Api")));
        }
    }
}
