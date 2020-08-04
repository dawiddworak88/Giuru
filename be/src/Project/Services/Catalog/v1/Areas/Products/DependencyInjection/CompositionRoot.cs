using Catalog.Api.v1.Areas.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.v1.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
