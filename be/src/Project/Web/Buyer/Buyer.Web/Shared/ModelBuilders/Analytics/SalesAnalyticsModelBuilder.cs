using Buyer.Web.Areas.Dashboard.Repositories;
using Buyer.Web.Shared.ViewModels.Analytics;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Analytics
{
    public class SalesAnalyticsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SalesAnalyticsViewModel>
    {
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;

        public SalesAnalyticsModelBuilder(
            IStringLocalizer<DashboardResources> dashboardResources,
            ISalesAnalyticsRepository salesAnalyticsRepository)
        {
            _dashboardResources = dashboardResources;
            _salesAnalyticsRepository = salesAnalyticsRepository;
        }

        public async Task<SalesAnalyticsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SalesAnalyticsViewModel
            {
                Title = _dashboardResources.GetString("NumberOfOrders")
            };

            var annualSales = await _salesAnalyticsRepository.GetAnnualSales(componentModel.Token, componentModel.Language);

            if (annualSales is not null)
            {
                var chartDataset = new List<double>();
                var chartLabels = new List<string>();

                foreach (var annualSalesItem in annualSales.OrEmptyIfNull())
                {
                    chartDataset.Add(annualSalesItem.Quantity);

                    var monthName = CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(annualSalesItem.Month);

                    chartLabels.Add($"{monthName.ToUpperInvariant()} - {annualSalesItem.Year}");
                }

                viewModel.ChartLables = chartLabels;
                viewModel.ChartDatasets = new List<SalesAnalyticsChartDatasetsViewModel>
                {
                    new SalesAnalyticsChartDatasetsViewModel
                    {
                        Data = chartDataset
                    }
                };
            }

            return viewModel;
        }
    }
}
