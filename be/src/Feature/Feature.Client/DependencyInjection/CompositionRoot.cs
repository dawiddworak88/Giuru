using Feature.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Client.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
        }
    }
}
