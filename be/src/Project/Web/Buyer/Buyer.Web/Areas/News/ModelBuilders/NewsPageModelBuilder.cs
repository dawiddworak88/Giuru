using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel> _newsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public NewsPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel> newsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _newsCatalogModelBuilder = newsCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _seoModelBuilder = seoModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<NewsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                NewsCatalog = await _newsCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
