using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Orders.DomainModels;
using System.Collections.Generic;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Foundation.GenericRepository.Definitions;
using Seller.Web.Areas.Orders.ComponetModels;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrdersPageCatalogModelBuilder : IAsyncComponentModelBuilder<OrdersPageComponentModel, CatalogViewModel<Order>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IStringLocalizer _globalLocalizer;
        private readonly IStringLocalizer _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public OrdersPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IOrdersRepository ordersRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _ordersRepository = ordersRepository;
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<Order>> BuildModelAsync(OrdersPageComponentModel componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<Order>, Order>();

            viewModel.Title = _orderLocalizer.GetString("Orders");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;
            viewModel.SearchTerm = componentModel.SearchTerm;

            viewModel.NewText = _orderLocalizer.GetString("NewOrder");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "OrdersApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(Order.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("ClientName"),
                    _globalLocalizer.GetString("OrderStatus"),
                    _globalLocalizer.GetString("LastModifiedDate"),
                    _globalLocalizer.GetString("CreatedDate")
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

            viewModel.PagedItems = await _ordersRepository.GetOrdersAsync(componentModel.Token, componentModel.Language, componentModel.SearchTerm, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(Order.CreatedDate)} desc");

            return viewModel;
        }
    }
}