using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using Buyer.Web.Shared.ModelBuilders.NotificationBar;

namespace Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts
{
    public class AvailableProductsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> _searchProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public AvailableProductsPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> searchProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _seoModelBuilder = seoModelBuilder;
            _headerModelBuilder = headerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _searchProductsCatalogModelBuilder = searchProductsCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<AvailableProductsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new AvailableProductsPageViewModel
            {
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Catalog = await _searchProductsCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel),
                Locale = componentModel.Language
            };

            return viewModel;
        }
    }
}
