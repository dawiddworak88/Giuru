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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ModelBuilders
{
    public class OrdersAnalyticsDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SalesAnalyticsViewModel> salesAnalyticsModelBuilder;
        private readonly ISalesAnalyticsRepository salesAnalyticsRepository;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<DashboardResources> dashboardResources;
        private readonly IStringLocalizer<GlobalResources> globalResources;

        public OrdersAnalyticsDetailModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SalesAnalyticsViewModel> salesAnalyticsModelBuilder,
            ISalesAnalyticsRepository salesAnalyticsRepository,
            IStringLocalizer<DashboardResources> dashboardResources,
            IStringLocalizer<GlobalResources> globalResources,
            IProductsRepository productsRepository)
        {
            this.salesAnalyticsModelBuilder = salesAnalyticsModelBuilder;
            this.salesAnalyticsRepository = salesAnalyticsRepository;
            this.productsRepository = productsRepository;
            this.dashboardResources = dashboardResources;
            this.globalResources = globalResources;
        }

        public async Task<OrdersAnalyticsDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrdersAnalyticsDetailViewModel
            {
                Title = this.dashboardResources.GetString("OrdersAnalysis"),
                TopOrderedProducts = this.dashboardResources.GetString("TopOrderedProducts"),
                NameLabel = this.dashboardResources.GetString("ProductName"),
                QuantityLabel = this.dashboardResources.GetString("ProductQuantity"),
                NoResultsLabel = this.globalResources.GetString("NoResultsLabel"),
                SalesAnalytics = await this.salesAnalyticsModelBuilder.BuildModelAsync(componentModel)
            };

            var salesProducts = await this.salesAnalyticsRepository.GetProductsSales(componentModel.Token, componentModel.Language);

            if (salesProducts is not null)
            {
                var products = await this.productsRepository.GetProductsAsync(salesProducts.Select(x => x.ProductId), null, null, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, componentModel.Token, null);

                var analyticsProducts = new List<OrderAnalyticsProductViewModel>();

                foreach (var salesProduct in salesProducts.OrEmptyIfNull())
                {
                    var analyticsProduct = new OrderAnalyticsProductViewModel
                    {
                        Sku = salesProduct.ProductSku,
                        Quantity = salesProduct.Quantity
                    };

                    var product = products?.Data?.FirstOrDefault(x => x.Id == salesProduct.ProductId);

                    if (product is not null)
                    {
                        analyticsProduct.Name = product.Name;
                    }

                    analyticsProducts.Add(analyticsProduct);
                }

                viewModel.Products = analyticsProducts;
            }

            return viewModel;
        }
    }
}
