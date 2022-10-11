using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrdersPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogOrderViewModel<Order>>
    {
        private readonly ICatalogOrderModelBuilder catalogModelBuilder;
        private readonly IOrdersRepository ordersRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer orderLocalizer;
        private readonly LinkGenerator linkGenerator;

        public OrdersPageCatalogModelBuilder(
            ICatalogOrderModelBuilder catalogModelBuilder,
            IOrdersRepository ordersRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.ordersRepository = ordersRepository;
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogOrderViewModel<Order>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogOrderViewModel<Order>, Order>();

            viewModel.Title = this.orderLocalizer.GetString("Orders");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = this.orderLocalizer.GetString("NewOrder");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Status", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "OrdersApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.OrderBy = $"{nameof(Order.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.globalLocalizer.GetString("ClientName"),
                    this.globalLocalizer.GetString("OrderStatus"),
                    this.globalLocalizer.GetString("LastModifiedDate"),
                    this.globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Order.ClientName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Order.OrderStatusName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Order.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Order.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.ordersRepository.GetOrdersAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(Order.CreatedDate)} desc");

            return viewModel;
        }
    }
}
