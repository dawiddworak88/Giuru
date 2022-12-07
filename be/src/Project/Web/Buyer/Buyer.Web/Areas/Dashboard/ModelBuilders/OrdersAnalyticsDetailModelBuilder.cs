using Buyer.Web.Areas.Dashboard.Repositories;
using Buyer.Web.Areas.Dashboard.ViewModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.PageContent.ComponentModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ModelBuilders
{
    public class OrdersAnalyticsDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel>
    {
        private readonly ISalesAnalyticsRepository salesAnalyticsRepository;
        private readonly IProductsRepository productsRepository;

        public OrdersAnalyticsDetailModelBuilder(
            ISalesAnalyticsRepository salesAnalyticsRepository,
            IProductsRepository productsRepository)
        {
            this.salesAnalyticsRepository = salesAnalyticsRepository;
            this.productsRepository = productsRepository;
        }

        public async Task<OrdersAnalyticsDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrdersAnalyticsDetailViewModel
            {
                Title = "test",
                NumberOfOrdersLabel = "asd",
                TopOrderedProducts = "Top zamówione produkty",
                NameLabel = "Nazwa",
                QuantityLabel = "Ilość"
            };

            var annualSales = await this.salesAnalyticsRepository.GetAnnualSales(componentModel.Token, componentModel.Language);

            if (annualSales is not null)
            {
                var chartDatasets = new List<double>();
                var a = new List<string>();

                foreach (var chartDataset in annualSales.OrEmptyIfNull())
                {
                    chartDatasets.Add(chartDataset.Quantity);

                    var monthName = CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(chartDataset.Month);
                    var year = chartDataset.Year.ToString().Substring(-2);

                    a.Add($"{monthName}. {year}");
                }

                viewModel.ChartLables = a;
                viewModel.ChartDatasets = new List<OrderAnalyticsChartDatasetsViewModel>
                {
                    new OrderAnalyticsChartDatasetsViewModel
                    {
                        Data = chartDatasets
                    }
                };
            }

            var salesProducts = await this.salesAnalyticsRepository.GetProductsSales(componentModel.Token, componentModel.Language);

            if (salesProducts is not null)
            {
                var products = await this.productsRepository.GetProductsAsync(salesProducts.Select(x => x.ProductId), null, null, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, componentModel.Token, null);

                var a = new List<OrderAnalyticsProductViewModel>();

                foreach (var product in products.Data.OrEmptyIfNull())
                {
                    var analyticsProduct = new OrderAnalyticsProductViewModel
                    {
                        Name = product.Name,
                        Sku = product.Sku,
                    };

                    var b = salesProducts.FirstOrDefault(x => x.ProductId == product.Id);

                    if (b is not null)
                    {
                        analyticsProduct.Quantity = b.Quantity;
                    }

                    a.Add(analyticsProduct);
                }

                viewModel.Products = a;
            }

            return viewModel;
        }
    }
}
