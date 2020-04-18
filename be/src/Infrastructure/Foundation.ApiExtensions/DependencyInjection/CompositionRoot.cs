using Foundation.ApiExtensions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.ApiExtensions.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterApiExtensionsDependencies(this IServiceCollection services)
        {
            services.AddScoped<IApiClientService, ApiClientService>();
        }
    }
}
