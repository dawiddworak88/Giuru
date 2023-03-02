using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Dashboard.Definitions;
using Seller.Web.Areas.Dashboard.Repositories;
using Seller.Web.Areas.Dashboard.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ModelBuilders
{
    public class DailySalesAnalyticsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DailySalesAnalyticsViewModel>
    {
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly IStringLocalizer<GlobalResources> _globalResources;
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;
        private readonly LinkGenerator _linkGenerator;

        public DailySalesAnalyticsModelBuilder(
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

        public async Task<DailySalesAnalyticsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var fromDate = DateTime.UtcNow.AddDays(DashboardConstants.DailyAnalyticsDifferenceInDays);
            var toDate = DateTime.UtcNow;

            var dailySales = await _salesAnalyticsRepository.GetDailySales(componentModel.Token, componentModel.Language, fromDate, toDate);

            if (dailySales is not null && dailySales.Any(x => x.Quantity > 0))
            {
                var viewModel = new DailySalesAnalyticsViewModel
                {
                    Title = _dashboardResources.GetString("DailySales"),
                    FromLabel = _dashboardResources.GetString("From"),
                    ToLabel = _dashboardResources.GetString("To"),
                    FromDate = fromDate,
                    ToDate = toDate,
                    DatePickerViews = DashboardConstants.FullDatePickerViews,
                    InvalidDateRangeErrorMessage = _dashboardResources.GetString("InvalidDateRange"),
                    GeneralErrorMessage = _globalResources.GetString("AnErrorOccurred"),
                    SaveUrl = _linkGenerator.GetPathByAction("Index", "DailySalesAnalyticsApi", new { Area = "Dashboard", culture = CultureInfo.CurrentUICulture.Name })
                };

                var chartDataset = new List<double>();
                var chartLabels = new List<string>();

                foreach (var dailySalesItem in dailySales.OrEmptyIfNull())
                {
                    chartDataset.Add(dailySalesItem.Quantity);

                    var monthNumber = dailySalesItem.Month.ToString();

                    if (dailySalesItem.Month < DashboardConstants.MonthNameUnderMonth)
                    {
                        monthNumber = $"0{monthNumber}";
                    }

                    chartLabels.Add($"{dailySalesItem.Day}.{monthNumber}");
                }

                viewModel.ChartLabels = chartLabels;
                viewModel.ChartDatasets = new List<ChartDatasetsViewModel>
                {
                    new ChartDatasetsViewModel
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
