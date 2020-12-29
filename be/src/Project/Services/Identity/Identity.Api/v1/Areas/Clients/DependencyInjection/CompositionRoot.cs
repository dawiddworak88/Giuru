using Identity.Api.v1.Areas.Clients.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.v1.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientsService, ClientsService>();
        }
    }
}