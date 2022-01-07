using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Localization.Extensions
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddCultureRouteConstraint(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(opts =>
                opts.ConstraintMap.Add(LocalizationConstants.CultureRouteConstraint, typeof(CultureRouteConstraint)));
        }
    }
}
