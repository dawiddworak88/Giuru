using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Dashboard.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ModelBuilders
{
    public class DashboardDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DashboardDetailViewModel>
    {
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DailySalesAnalyticsViewModel> _dailySalesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CountrySalesAnalyticsViewModel> _countriesSalesModelBuilder;

        public DashboardDetailModelBuilder(
            IStringLocalizer<DashboardResources> dashboardResources,
            IAsyncComponentModelBuilder<ComponentModelBase, DailySalesAnalyticsViewModel> dailySalesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CountrySalesAnalyticsViewModel> countriesSalesModelBuilder)
        {
            _dailySalesModelBuilder = dailySalesModelBuilder;
            _countriesSalesModelBuilder = countriesSalesModelBuilder;
            _dashboardResources = dashboardResources;
        }

        public async Task<DashboardDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DashboardDetailViewModel
            {
                Title = _dashboardResources.GetString("Dashboard"),
                DailySalesAnalytics = await _dailySalesModelBuilder.BuildModelAsync(componentModel),
                CountrySalesAnalytics = await _countriesSalesModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
