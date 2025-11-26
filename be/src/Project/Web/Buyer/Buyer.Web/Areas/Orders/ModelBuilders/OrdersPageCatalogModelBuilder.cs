using Buyer.Web.Areas.Orders.ComponentModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrdersPageCatalogModelBuilder : IAsyncComponentModelBuilder<OrdersPageComponentModel, CatalogOrderViewModel<Order>>
    {
        private readonly ICatalogOrderModelBuilder _catalogModelBuilder;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IStringLocalizer _globalLocalizer;
        private readonly IStringLocalizer _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public OrdersPageCatalogModelBuilder(
            ICatalogOrderModelBuilder catalogModelBuilder,
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

        public async Task<CatalogOrderViewModel<Order>> BuildModelAsync(OrdersPageComponentModel componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogOrderViewModel<Order>, Order>();

            viewModel.Title = _orderLocalizer.GetString("Orders");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;
            viewModel.SearchTerm = componentModel.SearchTerm;

            viewModel.NewText = _orderLocalizer.GetString("NewOrder");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Status", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "OrdersApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.AllLabel = _globalLocalizer.GetString("AllForFilters");
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

            var orderStatuses = await _ordersRepository.GetOrderStatusesAsync(componentModel.Token, componentModel.Language);

            var excludedStatusIds = new[] { OrdersConstants.OrderStatuses.HoldId, OrdersConstants.OrderStatuses.CancelId };

            viewModel.OrdersStatuses = orderStatuses
                .OrEmptyIfNull()
                .Where(os => !excludedStatusIds.Contains(os.Id))
                .Select(os => new ListItemViewModel
                {
                    Id = os.Id,
                    Name = os.Name
                });

            viewModel.PagedItems = await _ordersRepository.GetOrdersAsync(
                componentModel.Token, 
                componentModel.Language, 
                componentModel.SearchTerm, 
                Constants.DefaultPageIndex, 
                Constants.DefaultItemsPerPage, 
                $"{nameof(Order.CreatedDate)} desc", 
                null);

            return viewModel;
        }
    }
}
