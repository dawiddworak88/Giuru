using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Dashboard.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Dashboard.Repositories;
using System.Linq;
using Foundation.Extensions.ExtensionMethods;

namespace Seller.Web.Areas.Dashboard.ModelBuilders
{
    public class CountriesSalesAnalyticsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CountrySalesAnalyticsViewModel>
    {
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;

        public CountriesSalesAnalyticsModelBuilder(
            IStringLocalizer<DashboardResources> dashboardResources,
            ISalesAnalyticsRepository salesAnalyticsRepository)
        {
            _dashboardResources = dashboardResources;
            _salesAnalyticsRepository = salesAnalyticsRepository;
        }

        public async Task<CountrySalesAnalyticsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var countriesSales = await _salesAnalyticsRepository.GetCountriesSales(componentModel.Token, componentModel.Language);

            if (countriesSales is not null && countriesSales.Any(x => x.Quantity > 0))
            {
                var viewModel = new CountrySalesAnalyticsViewModel
                {
                    Title = string.Format(_dashboardResources.GetString("CountrySales").Value, 3)
                };

                var chartDataset = new List<double>();
                var chartLabels = new List<string>();

                foreach (var countrySalesItem in countriesSales.OrEmptyIfNull())
                {
                    chartDataset.Add(countrySalesItem.Quantity);
                    chartLabels.Add(countrySalesItem.Name);
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
