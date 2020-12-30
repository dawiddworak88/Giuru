using Foundation.Localization.Definitions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Content.Api.Configurations;

namespace Content.Api.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.Configure<LocalizationSettings>(configuration);
        }
    }
}
