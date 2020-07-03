using Foundation.TenantDatabase.Shared.Contexts;
using Foundation.TenantDatabase.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.TenantDatabase.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterTenantDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TenantGenericRepositoryFactory>();
            services.AddScoped<TenantDatabaseContextFactory>();

            services.AddDbContext<TenantDatabaseContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("TenantDatabaseContext"),
                    opt => opt.MigrationsAssembly("Foundation.TenantDatabase")));
        }
    }
}
