using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ModelBuilders;
using Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts;
using Buyer.Web.Areas.Products.ModelBuilders.Categories;
using Buyer.Web.Areas.Products.ModelBuilders.Products;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Repositories.Files;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.Services.SearchSuggestions;
using Buyer.Web.Areas.Products.Services.SearchSuggestions.ProductsSearchSuggestions;
using Buyer.Web.Areas.Products.Services.SearchSuggestions.StockLevelsSearchSuggetions;
using Buyer.Web.Areas.Products.ViewModels;
using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
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
            services.AddScoped<IMediaItemsRepository, MediaItemsRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IOutletRepository, OutletRepository>();

            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IProductsSearchSuggestionsService, ProductsSearchSuggestionsService>();
            services.AddScoped<IStockLevelsSearchSuggestionsService, StockLevelsSearchSuggestionsService>();
            services.AddScoped<ISearchSuggestionsService, SearchSuggestionsService>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsPageViewModel>, AvailableProductsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel>, AvailableProductsCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel>, CategoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel>, CategoryCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>, ProductPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>, ProductDetailModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel>, CategoryBreadcrumbsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductBreadcrumbsViewModel>, ProductBreadcrumbsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel>, OutletPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OutletPageCatalogViewModel>, OutletCatalogModelBuilder>();
        }
    }
}
