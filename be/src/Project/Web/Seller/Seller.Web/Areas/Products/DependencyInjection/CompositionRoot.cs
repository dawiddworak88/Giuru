using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Products.ModelBuilders;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Categories.ModelBuilders;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Categories.Repositories;

namespace Seller.Web.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductsRepository, ProductsRepository>();

            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel>, CategoriesPageModelBuilder> ();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryDetailFormViewModel>, CategoryDetailFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>, CategoriesPageCatalogModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductsPageViewModel>, ProductsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailFormViewModel>, ProductDetailFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>, ProductDetailPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Product>>, ProductsPageCatalogModelBuilder>();
        }
    }
}
