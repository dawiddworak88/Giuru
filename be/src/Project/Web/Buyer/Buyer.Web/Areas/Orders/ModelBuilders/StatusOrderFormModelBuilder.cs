using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.Repositories.OrderAttributes;
using Buyer.Web.Areas.Orders.Repositories.OrderAttributeValues;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class StatusOrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, StatusOrderFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> _filesModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderAttributesRepository _orderAttributesRepository;
        private readonly IOrderAttributeValuesRepository _orderAttributeValuesRepository;
        private readonly LinkGenerator _linkGenerator;

        public StatusOrderFormModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOrdersRepository ordersRepository,
            IOrderAttributesRepository orderAttributesRepository,
            IOrderAttributeValuesRepository orderAttributeValuesRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _ordersRepository = ordersRepository;
            _filesModelBuilder = filesModelBuilder;
            _clientLocalizer = clientLocalizer;
            _orderAttributesRepository = orderAttributesRepository;
            _orderAttributeValuesRepository = orderAttributeValuesRepository;
        }

        public async Task<StatusOrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new StatusOrderFormViewModel
            {
                Title = _orderLocalizer.GetString("Order"),
                MoreInfoLabel = _orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = _orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = _orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = _orderLocalizer.GetString("QuantityLabel"),
                ExternalReferenceLabel = _orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = _orderLocalizer.GetString("SkuLabel"),
                OrderStatusLabel = _orderLocalizer.GetString("OrderStatus"),
                ExpectedDeliveryLabel = _orderLocalizer.GetString("ExpectedDeliveryLabel"),
                FabricsLabel = _orderLocalizer.GetString("FabricsLabel"),
                CancelOrderLabel = _orderLocalizer.GetString("CancelOrder"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                UpdateOrderStatusUrl = _linkGenerator.GetPathByAction("Cancel", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                CustomOrderLabel = _globalLocalizer.GetString("CustomOrderLabel"),
                OutletQuantityLabel = _orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = _orderLocalizer.GetString("StockQuantityLabel"),
                YesLabel = _globalLocalizer.GetString("Yes"),
                NoLabel = _globalLocalizer.GetString("No"),
                CancelationConfirmationDialogLabel = _orderLocalizer.GetString("CancelationConfirmationDialog"),
                AreYouSureToCancelOrderLabel = _orderLocalizer.GetString("AreYouSureToCancelOrder"),
                OrdersUrl = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToOrders = _orderLocalizer.GetString("NavigateToOrdersList"),
                DeliveryAddressLabel = _clientLocalizer.GetString("DeliveryAddress"),
                BillingAddressLabel = _clientLocalizer.GetString("BillingAddress"),
                ExpectedDateOfProductOnStockLabel = _orderLocalizer.GetString("ExpectedDateOfProductOnStock")
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
                    viewModel.CustomOrder = order.MoreInfo;
                    viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "OrderItem", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
                    viewModel.CanCancelOrder = false;
                    viewModel.OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Sku = x.ProductSku,
                        Name = x.ProductName,
                        ProductUrl = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        MoreInfo = x.MoreInfo,
                        OrderItemStatusId = x.OrderItemStatusId,
                        OrderItemStatusName = x.OrderItemStatusName,
                        ExpectedDateOfProductOnStock = x.OrderItemStatusChangeComment,
                        ProductAttributes = x.ProductAttributes,
                        ImageAlt = x.ProductName,
                        ImageSrc = x.PictureUrl
                    });

                    var orderAttributes = await _orderAttributesRepository.GetAsync(componentModel.Token, componentModel.Language);

                    if (orderAttributes is not null)
                    {
                        var orderAttributeValues = await _orderAttributeValuesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                        viewModel.OrderAttributes = orderAttributes.Select(x =>
                        {
                            var attributeValue = orderAttributeValues.FirstOrDefault(y => y.AttributeId == x.Id);

                            return new OrderAttributeViewModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Type = x.Type,
                                Value = attributeValue?.Value,
                                IsRequired = x.IsRequired,
                                Options = x.Options.Select(x => new OrderAttributeOptionViewModel
                                {
                                    Name = x.Name,
                                    Value = x.Value
                                })
                            };
                        });
                    }
                }

                if (order.ShippingAddressId.HasValue)
                {
                    viewModel.DeliveryAddress = $"{order.ShippingCompany}, {order.ShippingFirstName} {order.ShippingLastName}, {order.ShippingPostCode} {order.ShippingCity}";
                }

                if (order.BillingAddressId.HasValue)
                {
                    viewModel.BillingAddress = $"{order.BillingCompany}, {order.BillingFirstName} {order.BillingLastName}, {order.BillingPostCode} {order.BillingCity}";
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
