using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Localization.Extensions
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalizationConfiguration>(configuration.GetSection("Localization"));
        }

        public static void AddCultureRouteConstraint(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(opts =>
                opts.ConstraintMap.Add(LocalizationConstants.CultureRouteConstraint, typeof(CultureRouteConstraint)));
        }
    }
}
