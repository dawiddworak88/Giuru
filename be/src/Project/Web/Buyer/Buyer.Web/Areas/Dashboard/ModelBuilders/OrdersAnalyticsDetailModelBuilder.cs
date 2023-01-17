using Buyer.Web.Areas.Dashboard.Constants;
using Buyer.Web.Areas.Dashboard.DomainModels;
using Buyer.Web.Areas.Dashboard.Repositories;
using Buyer.Web.Areas.Dashboard.ViewModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ModelBuilders
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
                TopOrderedProducts = _dashboardResources.GetString("TopOrderedProducts"),
                NameLabel = _dashboardResources.GetString("ProductName"),
                QuantityLabel = _dashboardResources.GetString("ProductQuantity"),
                NoResultsLabel = _globalResources.GetString("NoResultsLabel"),
                SalesAnalytics = await _salesAnalyticsModelBuilder.BuildModelAsync(componentModel)
            };

            var salesProducts = await _salesAnalyticsRepository.GetProductsSales(componentModel.Token, componentModel.Language, DashboardConstants.SalesAnalytics.DefaultSalesSize, $"{nameof(Product.Quantity)} desc");

            if (salesProducts is not null)
            {
                var analyticsProducts = new List<OrderAnalyticsProductViewModel>();

                foreach (var salesProduct in salesProducts.OrEmptyIfNull())
                {
                    var analyticsProduct = new OrderAnalyticsProductViewModel
                    {
                        Sku = salesProduct.ProductSku,
                        Quantity = salesProduct.Quantity,
                        Name = salesProduct.ProductName,
                    };

                    analyticsProducts.Add(analyticsProduct);
                }

                viewModel.Products = analyticsProducts;
            }

            return viewModel;
        }
    }
}
