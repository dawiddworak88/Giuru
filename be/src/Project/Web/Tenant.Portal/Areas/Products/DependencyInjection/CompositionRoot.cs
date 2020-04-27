using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Portal.Areas.Products.ModelBuilders;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<ProductPageViewModel>, ProductPageModelBuilder>();
            services.AddScoped<IModelBuilder<ProductCatalogViewModel>, ProductCatalogModelBuilder>();
            services.AddScoped<IModelBuilder<ProductDetailPageViewModel>, ProductDetailPageModelBuilder>();
        }
    }
}
