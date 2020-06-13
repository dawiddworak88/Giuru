using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Portal.Areas.Products.ComponentModels;
using Tenant.Portal.Areas.Products.ModelBuilders;
using Tenant.Portal.Areas.Products.Repositories;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductSchemaRepository, ProductSchemaRepository>();
            services.AddScoped<IAsyncComponentModelBuilder<ProductsComponentModel, ProductPageViewModel>, ProductPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ProductsCatalogComponentModel, ProductPageCatalogViewModel>, ProductPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ProductDetailFormComponentModel, ProductDetailFormViewModel>, ProductDetailFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ProductDetailComponentModel, ProductDetailPageViewModel>, ProductDetailPageModelBuilder>();
        }
    }
}
