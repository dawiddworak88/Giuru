using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryPageModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel>
    {
        private readonly IModelBuilder<BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel> categoryBreadcrumbsModelBuilder;
        private readonly IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> categoryCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public CategoryPageModelBuilder(
            IModelBuilder<BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel> categoryBreadcrumbsModelBuilder,
            IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> categoryCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.categoryBreadcrumbsModelBuilder = categoryBreadcrumbsModelBuilder;
            this.categoryCatalogModelBuilder = categoryCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<CategoryPageViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = new CategoryPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Breadcrumbs = await this.categoryBreadcrumbsModelBuilder.BuildModelAsync(componentModel),
                Catalog = await this.categoryCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
