using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryPageModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel> _categoryBreadcrumbsModelBuilder;
        private readonly IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> _categoryCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public CategoryPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel> categoryBreadcrumbsModelBuilder,
            IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> categoryCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _categoryBreadcrumbsModelBuilder = categoryBreadcrumbsModelBuilder;
            _categoryCatalogModelBuilder = categoryCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<CategoryPageViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = new CategoryPageViewModel
            {
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Breadcrumbs = await _categoryBreadcrumbsModelBuilder.BuildModelAsync(componentModel),
                Catalog = await _categoryCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
