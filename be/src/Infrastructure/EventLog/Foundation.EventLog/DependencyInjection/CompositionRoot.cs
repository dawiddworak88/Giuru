using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.EventLog.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.EventLog.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterEventLogDependencies(this IServiceCollection services)
        {
            services.AddScoped<IApiClientService, ApiClientService>();
            services.AddScoped<IEventLogRepository, EventLogRepository>();
        }
    }
}
