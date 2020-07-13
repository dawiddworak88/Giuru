using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Portal.Areas.Products.ModelBuilders;
using Seller.Portal.Areas.Products.Repositories;
using Seller.Portal.Areas.Products.ViewModels;
using Seller.Portal.Shared.ComponentModels;

namespace Seller.Portal.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductSchemaRepository, ProductSchemaRepository>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>, ProductPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductPageCatalogViewModel>, ProductPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailFormViewModel>, ProductDetailFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailPageViewModel>, ProductDetailPageModelBuilder>();
        }
    }
}
