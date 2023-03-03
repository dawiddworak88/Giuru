using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
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
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;

        public DailySalesAnalyticsModelBuilder(
            IStringLocalizer<DashboardResources> dashboardResources,
            ISalesAnalyticsRepository salesAnalyticsRepository)
        {
            _dashboardResources = dashboardResources;
            _salesAnalyticsRepository = salesAnalyticsRepository;
        }

        public async Task<DailySalesAnalyticsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var dailySales = await _salesAnalyticsRepository.GetDailySales(componentModel.Token, componentModel.Language);

            if (dailySales is not null && dailySales.Any(x => x.Quantity > 0))
            {
                var viewModel = new DailySalesAnalyticsViewModel
                {
                    Title = _dashboardResources.GetString("DailySales")
                };

                var chartDataset = new List<double>();
                var chartLabels = new List<string>();

                foreach (var dailySalesItem in dailySales.OrEmptyIfNull())
                {
                    chartDataset.Add(dailySalesItem.Quantity);

                    var dayName = CultureInfo.CurrentUICulture.DateTimeFormat.GetDayName(((DayOfWeek)dailySalesItem.DayOfWeek));

                    var monthNumber = dailySalesItem.Month.ToString();

                    if (dailySalesItem.Month < 10)
                    {
                        monthNumber = $"0{monthNumber}";
                    }

                    chartLabels.Add($"{dayName.ToUpperInvariant()} - {dailySalesItem.Day}.{monthNumber}");
                }

                viewModel.ChartLables = chartLabels;
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
