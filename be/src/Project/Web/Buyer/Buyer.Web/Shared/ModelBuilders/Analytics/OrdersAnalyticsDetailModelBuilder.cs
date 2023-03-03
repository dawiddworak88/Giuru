using Buyer.Web.Areas.Dashboard.Repositories;
using Buyer.Web.Shared.ViewModels.Analytics;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Analytics
{
    public class OrdersAnalyticsDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SalesAnalyticsViewModel> _salesAnalyticsModelBuilder;
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly IStringLocalizer<GlobalResources> _globalResources;

        public OrdersAnalyticsDetailModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SalesAnalyticsViewModel> salesAnalyticsModelBuilder,
            ISalesAnalyticsRepository salesAnalyticsRepository,
            IStringLocalizer<DashboardResources> dashboardResources,
            IStringLocalizer<GlobalResources> globalResources)
        {
            _salesAnalyticsModelBuilder = salesAnalyticsModelBuilder;
            _salesAnalyticsRepository = salesAnalyticsRepository;
            _dashboardResources = dashboardResources;
            _globalResources = globalResources;
        }

        public async Task<OrdersAnalyticsDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrdersAnalyticsDetailViewModel
            {
                Title = _dashboardResources.GetString("OrdersAnalysis"),
                NameLabel = _dashboardResources.GetString("ProductName"),
                QuantityLabel = _dashboardResources.GetString("ProductQuantity"),
                NoResultsLabel = _globalResources.GetString("NoResultsLabel"),
                SalesAnalytics = await _salesAnalyticsModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}