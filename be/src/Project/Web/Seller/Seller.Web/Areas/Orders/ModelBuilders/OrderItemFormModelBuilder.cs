using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.Repositories.OrderAttributes;
using Seller.Web.Areas.Orders.Repositories.OrderAttributeValues;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> _orderItemStatusChangesModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderAttributesRepository _orderAttributesRepository;
        private readonly IOrderAttributeValuesRepository _orderAttributeValuesRepository;
        private readonly LinkGenerator _linkGenerator;

        public OrderItemFormModelBuilder
        (
            IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> orderItemStatusChangesModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IOrdersRepository ordersRepository,
            IOrderAttributesRepository orderAttributesRepository,
            IOrderAttributeValuesRepository orderAttributeValuesRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _ordersRepository = ordersRepository;
            _orderItemStatusChangesModelBuilder = orderItemStatusChangesModelBuilder;
            _orderAttributesRepository = orderAttributesRepository;
            _orderAttributeValuesRepository = orderAttributeValuesRepository;
        }

        public async Task<OrderItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _orderLocalizer.GetString("EditOrderItem"),
                SkuLabel = _globalLocalizer.GetString("Sku"),
                NameLabel = _globalLocalizer.GetString("Name"),
                OrderStatusLabel = _orderLocalizer.GetString("OrderStatus"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NavigateToOrderLabel = _orderLocalizer.GetString("NavigateToOrder"),
                ExpectedDateOfProductOnStockLabel = _orderLocalizer.GetString("ExpectedDateOfProductOnStock"),
                SaveUrl = _linkGenerator.GetPathByAction("Item", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                QuantityLabel = _orderLocalizer.GetString("QuantityLabel"),
                OutletQuantityLabel = _orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = _orderLocalizer.GetString("StockQuantityLabel"),
                ExternalReferenceLabel = _orderLocalizer.GetString("ExternalReferenceLabel"),
                MoreInfoLabel = _orderLocalizer.GetString("MoreInfoLabel"),
                CancelOrderItemLabel = _orderLocalizer.GetString("CancelOrder"),
                CancelOrderItemStatusUrl = _linkGenerator.GetPathByAction("CancelOrderItem", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var orderStatuses = await _ordersRepository.GetOrderStatusesAsync(componentModel.Token, componentModel.Language);

                if (orderStatuses is not null)
                {
                    viewModel.OrderItemStatuses = orderStatuses.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
                }

                var orderItem = await _ordersRepository.GetOrderItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (orderItem is not null)
                {
                    viewModel.Id = orderItem.Id;
                    viewModel.ProductName = orderItem.ProductName;
                    viewModel.ProductSku = orderItem.ProductSku;
                    viewModel.OrderItemStatusId = orderItem.OrderItemStatusId;
                    viewModel.Quantity = orderItem.Quantity;
                    viewModel.StockQuantity = orderItem.StockQuantity;
                    viewModel.OutletQuantity = orderItem.OutletQuantity;
                    viewModel.ImageUrl = orderItem.PictureUrl;
                    viewModel.ImageAlt = orderItem.ProductName;
                    viewModel.OrderUrl = _linkGenerator.GetPathByAction("Edit", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, id = orderItem.OrderId });
                    viewModel.ExternalReference = orderItem.ExternalReference;
                    viewModel.MoreInfo = orderItem.MoreInfo;
                    viewModel.ExpectedDateOfProductOnStock = orderItem.OrderItemStatusChangeComment;

                    if (orderItem.OrderItemStatusId == OrdersConstants.OrderStatuses.NewId || 
                        orderItem.OrderItemStatusId == Guid.Empty)
                    {
                        viewModel.CanCancelOrderItem = true;
                    }

                    var orderAttributes = await _orderAttributesRepository.GetAsync(componentModel.Token, componentModel.Language);

                    if (orderAttributes is not null)
                    {
                        var orderAttributesValues = await _orderAttributeValuesRepository.GetAsync(componentModel.Token, componentModel.Language, orderItem.OrderId);

                        viewModel.OrderAttributes = orderAttributes.Select(x =>
                        {
                            var orderAttributeValue = orderAttributesValues.FirstOrDefault(y => y.AttributeId == x.Id);

                            return new OrderAttributeViewModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Type = x.Type,
                                Value = orderAttributeValue?.Value,
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

                if (orderItem.LastOrderItemStatusChangeId is not null)
                {
                    var orderItemStatusChanges = await _ordersRepository.GetOrderItemStatusesAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                    if (orderItemStatusChanges is not null)
                    {
                        viewModel.StatusChanges = orderItemStatusChanges.StatusChanges.Select(x => new OrderItemStatusChangeViewModel
                        {
                            OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                            OrderItemStatusName = x.OrderItemStatusName,
                            CreatedDate = x.CreatedDate
                        });
                    }

                    viewModel.OrderItemStatusChanges = await _orderItemStatusChangesModelBuilder.BuildModelAsync(componentModel);
                }
            }
            
            return viewModel;
        }
    }
}
