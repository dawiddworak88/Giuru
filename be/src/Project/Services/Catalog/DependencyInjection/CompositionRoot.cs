using Catalog.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CatalogContext>();

            services.AddDbContext<CatalogContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("CatalogContext"),
                    opt => opt.MigrationsAssembly("Foundation.Database").UseNetTopologySuite()));
        }
    }
}
