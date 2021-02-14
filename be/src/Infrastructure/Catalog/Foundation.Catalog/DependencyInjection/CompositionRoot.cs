using Foundation.Catalog.Repositories.Products.ProductIndexingRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Catalog.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterCatalogBaseDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductIndexingRepository, ProductIndexingRepository>();
        }
    }
}
