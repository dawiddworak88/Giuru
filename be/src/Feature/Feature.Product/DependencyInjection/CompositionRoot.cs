using Feature.Product.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Product.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
