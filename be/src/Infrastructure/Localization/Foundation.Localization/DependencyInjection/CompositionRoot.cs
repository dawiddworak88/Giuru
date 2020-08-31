using Foundation.Localization.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Localization.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterBaseLocalizationDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICultureService, CultureService>();
        }
    }
}
