using Buyer.Web.Areas.Dashboard.Definitions;
using Buyer.Web.Areas.Dashboard.Repositories;
using Buyer.Web.Shared.ViewModels.Analytics;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Analytics
{
    public class SalesAnalyticsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SalesAnalyticsViewModel>
    {
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly IStringLocalizer<GlobalResources> _globalResources;
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;
        private readonly LinkGenerator _linkGenerator;

        public SalesAnalyticsModelBuilder(
            IStringLocalizer<DashboardResources> dashboardResources,
            ISalesAnalyticsRepository salesAnalyticsRepository,
            IStringLocalizer<GlobalResources> globalResources,
            LinkGenerator linkGenerator)
        {
            _dashboardResources = dashboardResources;
            _salesAnalyticsRepository = salesAnalyticsRepository;
            _linkGenerator = linkGenerator;
            _globalResources = globalResources;
        }

        public async Task<SalesAnalyticsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var fromDate = DateTime.UtcNow.AddMonths(DashboardConstants.AnnualAnalyticsDifferenceInMonths);
            var toDate = DateTime.UtcNow;

            var annualSales = await _salesAnalyticsRepository.GetAnnualSales(componentModel.Token, componentModel.Language, fromDate, toDate);

            if (annualSales is not null && annualSales.Any(x => x.Quantity > 0))
            {
                var viewModel = new SalesAnalyticsViewModel
                {
                    FromLabel = _dashboardResources.GetString("From"),
                    ToLabel = _dashboardResources.GetString("To"),
                    FromDate = fromDate,
                    ToDate = toDate,
                    InvalidDateRangeErrorMessage = _dashboardResources.GetString("InvalidDateRange"),
                    GeneralErrorMessage = _globalResources.GetString("AnErrorOccurred"),
                    SaveUrl = _linkGenerator.GetPathByAction("Index", "SalesAnalyticsApi", new { Area = "Dashboard", culture = CultureInfo.CurrentUICulture.Name })
                };

                var chartDataset = new List<double>();
                var chartLabels = new List<string>();

                foreach (var annualSalesItem in annualSales.OrEmptyIfNull())
                {
                    chartDataset.Add(annualSalesItem.Quantity);

                    var monthName = CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(annualSalesItem.Month);

                    chartLabels.Add($"{monthName.ToUpperInvariant()} - {annualSalesItem.Year}");
                }

                viewModel.ChartLabels = chartLabels;
                viewModel.ChartDatasets = new List<SalesAnalyticsChartDatasetsViewModel>
                {
                    new SalesAnalyticsChartDatasetsViewModel
                    {
                        Data = chartDataset
                    }
                };

                return viewModel;
            }

            return default;
        }
    }
}
