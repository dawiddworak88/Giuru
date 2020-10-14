using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ModelBuilders.Brands;
using Buyer.Web.Areas.Products.ModelBuilders.Categories;
using Buyer.Web.Areas.Products.ModelBuilders.Products;
using Buyer.Web.Areas.Products.Repositories.Brands;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductDependencies(this IServiceCollection services)
        {
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<IAsyncComponentModelBuilder<CategoryComponentModel, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<CategoryComponentModel, CategoryCatalogViewModel>, CategoryCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>, ProductPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>, ProductDetailModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, BrandCatalogViewModel>, BrandCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, BrandDetailViewModel>, BrandDetailModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, BrandPageViewModel>, BrandPageModelBuilder>();
        }
    }
}
