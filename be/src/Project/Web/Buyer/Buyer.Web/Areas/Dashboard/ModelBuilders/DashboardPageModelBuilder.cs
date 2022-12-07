using Buyer.Web.Areas.Dashboard.ViewModels;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ModelBuilders
{
    public class DashboardPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel> ordersAnalyticsDetailModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DashboardPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel> ordersAnalyticsDetailModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.ordersAnalyticsDetailModelBuilder = ordersAnalyticsDetailModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<DashboardPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DashboardPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                OrdersAnalyticsDetail = await this.ordersAnalyticsDetailModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
