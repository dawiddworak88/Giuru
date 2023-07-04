using Buyer.Web.Areas.Dashboard.ViewModels;
using Buyer.Web.Shared.ViewModels.Analytics;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ModelBuilders
{
    public class DashboardPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel> _ordersAnalyticsDetailModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;

        public DashboardPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel> ordersAnalyticsDetailModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder)
        {
            _seoModelBuilder = seoModelBuilder;
            _headerModelBuilder = headerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _ordersAnalyticsDetailModelBuilder = ordersAnalyticsDetailModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<DashboardPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DashboardPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                OrdersAnalyticsDetail = await _ordersAnalyticsDetailModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
