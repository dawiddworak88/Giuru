using Foundation.TenantDatabase.Shared.Contexts;
using Foundation.TenantDatabase.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.TenantDatabase.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterTenantDatabaseDependencies(this IServiceCollection services)
        {
            services.AddScoped<TenantGenericRepositoryFactory>();
            services.AddScoped<TenantDatabaseContextFactory>();
        }
    }
}
