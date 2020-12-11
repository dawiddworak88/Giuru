using Catalog.Api.v1.Areas.Products.Repositories.ProductIndexingRepositories;
using Catalog.Api.v1.Areas.Products.Repositories.ProductSearchRepositories;
using Catalog.Api.v1.Areas.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.v1.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductIndexingRepository, ProductIndexingRepository>();
            services.AddScoped<IProductSearchRepository, ProductSearchRepository>();
            services.AddScoped<IProductsService, ProductsService>();
        }
    }
}
