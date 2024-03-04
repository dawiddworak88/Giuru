using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsItemPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsItemBreadcrumbsViewModel> _breadcrumbsModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel> _newsDetailsModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public NewsItemPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel> newsDetailsModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsItemBreadcrumbsViewModel> breadcrumbsModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _newsDetailsModelBuilder = newsDetailsModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _breadcrumbsModelBuilder = breadcrumbsModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<NewsItemPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                Breadcrumbs = await _breadcrumbsModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                NewsItemDetails = await _newsDetailsModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
