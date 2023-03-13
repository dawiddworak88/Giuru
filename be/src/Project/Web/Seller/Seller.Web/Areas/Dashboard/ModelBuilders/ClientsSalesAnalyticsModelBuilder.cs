using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Dashboard.Definitions;
using Seller.Web.Areas.Dashboard.DomainModels;
using Seller.Web.Areas.Dashboard.Repositories;
using Seller.Web.Areas.Dashboard.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ModelBuilders
{
    public class ClientsSalesAnalyticsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientsSalesAnalyticsViewModel>
    {
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly IStringLocalizer<GlobalResources> _globalResources;
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;
        private readonly LinkGenerator _linkGenerator;

        public ClientsSalesAnalyticsModelBuilder(
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

        public async Task<ClientsSalesAnalyticsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var fromDate = DateTime.UtcNow.AddMonths(DashboardConstants.ClientsAnalyticsDifferenceInMonths);
            var toDate = DateTime.UtcNow;

            var clientsSales = await _salesAnalyticsRepository.GetTopClientsSales(componentModel.Token, componentModel.Language, fromDate, toDate, DashboardConstants.DefaultClientsSalesSize);

            if (clientsSales is not null)
            {
                var viewModel = new ClientsSalesAnalyticsViewModel
                {
                    Title = _dashboardResources.GetString("TopClientsSales"),
                    FromLabel = _dashboardResources.GetString("From"),
                    ToLabel = _dashboardResources.GetString("To"),
                    FromDate = fromDate,
                    ToDate = toDate,
                    DatePickerViews = DashboardConstants.FullDatePickerViews,
                    InvalidDateRangeErrorMessage = _dashboardResources.GetString("InvalidDateRange"),
                    GeneralErrorMessage = _globalResources.GetString("AnErrorOccurred"),
                    SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientsSalesAnalyticsApi", new { Area = "Dashboard", culture = CultureInfo.CurrentUICulture.Name }),
                    NameLabel = _globalResources.GetString("Name"),
                    SkuLabel = _globalResources.GetString("Sku"),
                    QuantityLabel = _globalResources.GetString("Quantity")
                };

                viewModel.Clients = clientsSales.Select(x => new ClientSalesAnalyticsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity
                });

                return viewModel;
            }

            return default;
        }
    }
}