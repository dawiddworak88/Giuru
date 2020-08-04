using Catalog.Api.v1.Areas.Clients.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.v1.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
        }
    }
}
