using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Services.ApiResponseServices;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.ApiExtensions.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterApiExtensionsDependencies(this IServiceCollection services)
        {
            services.AddScoped<IApiClientService, ApiClientService>();
            services.AddScoped<IApiResponseService, ApiResponseService>();
        }
    }
}
