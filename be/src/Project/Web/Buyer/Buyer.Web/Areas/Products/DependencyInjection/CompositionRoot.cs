using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ModelBuilders;
using Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts;
using Buyer.Web.Areas.Products.ModelBuilders.Categories;
using Buyer.Web.Areas.Products.ModelBuilders.Products;
using Buyer.Web.Areas.Products.ModelBuilders.SearchProducts;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Repositories.Files;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels;
using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Products.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterProductDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMediaItemsRepository, MediaItemsRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IOutletRepository, OutletRepository>();

            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsPageViewModel>, SearchProductsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel>, SearchProductsCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<PriceComponentModel, AvailableProductsPageViewModel>, AvailableProductsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<PriceComponentModel, AvailableProductsCatalogViewModel>, AvailableProductsCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel>, CategoryCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<PriceComponentModel, ProductPageViewModel>, ProductPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<PriceComponentModel, ProductDetailViewModel>, ProductDetailModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel>, CategoryBreadcrumbsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductBreadcrumbsViewModel>, ProductBreadcrumbsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<PriceComponentModel, OutletPageViewModel>, OutletPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<PriceComponentModel, OutletPageCatalogViewModel>, OutletCatalogModelBuilder>();
        }
    }
}
