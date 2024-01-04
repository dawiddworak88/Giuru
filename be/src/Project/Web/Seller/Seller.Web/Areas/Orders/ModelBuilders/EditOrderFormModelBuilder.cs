using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ComponetModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.ComponentModels.Files;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class EditOrderFormModelBuilder : IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> _filesModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOrdersRepository _ordersRepository;

        public EditOrderFormModelBuilder
        (
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator,
            IOrdersRepository ordersRepository)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _ordersRepository = ordersRepository;
            _filesModelBuilder = filesModelBuilder;
            _clientLocalizer = clientLocalizer;
        }

        public async Task<EditOrderFormViewModel> BuildModelAsync(OrdersPageComponentModel componentModel)
        {
            var viewModel = new EditOrderFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _orderLocalizer.GetString("EditOrder"),
                MoreInfoLabel = _orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = _orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = _orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = _orderLocalizer.GetString("QuantityLabel"),
                ExternalReferenceLabel = _orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = _orderLocalizer.GetString("SkuLabel"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                OrderStatusLabel = _orderLocalizer.GetString("OrderStatus"),
                SaveText = _orderLocalizer.GetString("UpdateOrderStatus"),
                ClientLabel = _globalLocalizer.GetString("Client"),
                CancelOrderLabel = _orderLocalizer.GetString("CancelOrder"),
                CancelOrderStatusUrl = _linkGenerator.GetPathByAction("Cancel", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                OutletQuantityLabel = _orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = _orderLocalizer.GetString("StockQuantityLabel"),
                CustomOrderLabel = _globalLocalizer.GetString("CustomOrderLabel"),
                UpdateOrderItemStatusUrl = _linkGenerator.GetPathByAction("Item", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                OrdersUrl = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, searchTerm = componentModel.SearchTerm }),
                NavigateToOrders = _orderLocalizer.GetString("NavigateToOrdersList"),
                DeliveryAddressLabel = _clientLocalizer.GetString("DeliveryAddress"),
                BillingAddressLabel = _clientLocalizer.GetString("BillingAddress"),
                SearchTerm = componentModel.SearchTerm
            };

            var orderStatuses = await _ordersRepository.GetOrderStatusesAsync(componentModel.Token, componentModel.Language);

            if (orderStatuses is not null)
            {
                viewModel.OrderStatuses = orderStatuses.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }
            
            if (componentModel.Id.HasValue)
            {
                var order = await _ordersRepository.GetOrderAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (order is not null)
                {
                    viewModel.Id = order.Id;
                    viewModel.OrderStatusId = order.OrderStatusId;
                    viewModel.ClientUrl = _linkGenerator.GetPathByAction("Edit", "Client", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, Id = order.ClientId });
                    viewModel.ClientName = order.ClientName;
                    viewModel.UpdateOrderStatusUrl = _linkGenerator.GetPathByAction("Index", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, Id = order.ClientId });
                    viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "OrderItem", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
                    viewModel.OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Sku = x.ProductSku,
                        Name = x.ProductName,
                        ProductUrl = _linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        MoreInfo = x.MoreInfo,
                        OrderItemStatusId = x.OrderItemStatusId,
                        OrderItemStatusName = x.OrderItemStatusName,
                        ExpectedDateOfProductOnStock = x.OrderItemStatusChangeComment,
                        ImageAlt = x.ProductName,
                        ImageSrc = x.PictureUrl
                    });
                    viewModel.CustomOrder = order.MoreInfo;

                    if (order.ShippingAddressId is not null)
                    {
                        viewModel.DeliveryAddress = $"{order.ShippingCompany}, {order.ShippingFirstName} {order.ShippingLastName}, {order.ShippingPostCode} {order.ShippingCity}";
                    }

                    if (order.BillingAddressId is not null)
                    {
                        viewModel.BillingAddress = $"{order.BillingCompany}, {order.BillingFirstName} {order.BillingLastName}, {order.BillingPostCode} {order.BillingCity}";
                    }

                    if (order.OrderStatusId == OrdersConstants.OrderStatuses.NewId)
                    {
                        viewModel.CanCancelOrder = true;
                    }

                    var orderItemsStatuses = new List<OrderItemStatusViewModel>();

                    foreach (var orderItem in order.OrderItems.OrEmptyIfNull())
                    {
                        orderItemsStatuses.Add(new OrderItemStatusViewModel
                        {
                            Id = orderItem.Id,
                            OrderStatusId = orderItem.OrderItemStatusId
                        });
                    }

                    viewModel.OrderItemsStatuses = orderItemsStatuses;
                }

                var orderFiles = await _ordersRepository.GetOrderFilesAsync(componentModel.Token, componentModel.Language, componentModel.Id, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, null, $"{nameof(OrderFile.CreatedDate)} desc");

                if (orderFiles is not null)
                {
                    var filesComponentModel = new FilesComponentModel
                    {
                        Id = componentModel.Id,
                        IsAuthenticated = componentModel.IsAuthenticated,
                        Language = componentModel.Language,
                        Token = componentModel.Token,
                        SearchApiUrl = _linkGenerator.GetPathByAction("GetFiles", "OrderFileApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                        Files = orderFiles.Data.OrEmptyIfNull().Select(x => x.Id)
                    };

                    viewModel.Attachments = await _filesModelBuilder.BuildModelAsync(filesComponentModel);
                }
            }

            return viewModel;
        }
    }
} 