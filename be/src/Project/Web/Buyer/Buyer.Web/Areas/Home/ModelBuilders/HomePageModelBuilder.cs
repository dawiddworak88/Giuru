using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.ComponentModels;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Headers;
using System.Globalization;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Buyer.Web.Areas.Home.ViewModel;
using Buyer.Web.Shared.ViewModels.Analytics;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel> _heroSliderModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel> _carouselGridModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel> _contentGridModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageNewsCarouselGridViewModel> _newsModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel> _ordersAnalyticsModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public HomePageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel> heroSliderModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel> carouselGridModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel> contentGridModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageNewsCarouselGridViewModel> newsModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel> ordersAnalyticsModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _seoModelBuilder = seoModelBuilder;
            _headerModelBuilder = headerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _heroSliderModelBuilder = heroSliderModelBuilder;
            _carouselGridModelBuilder = carouselGridModelBuilder;
            _contentGridModelBuilder = contentGridModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _newsModelBuilder = newsModelBuilder;
            _ordersAnalyticsModelBuilder = ordersAnalyticsModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<HomePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new HomePageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                HeroSlider = await _heroSliderModelBuilder.BuildModelAsync(componentModel),
                OrdersAnalytics = await _ordersAnalyticsModelBuilder.BuildModelAsync(componentModel),
                CarouselGrid = await _carouselGridModelBuilder.BuildModelAsync(componentModel),
                ContentGrid = await _contentGridModelBuilder.BuildModelAsync(componentModel),
                NewsCarouselGrid = await _newsModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
