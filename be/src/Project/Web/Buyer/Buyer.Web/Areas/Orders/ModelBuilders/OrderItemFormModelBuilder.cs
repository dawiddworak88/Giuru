using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ViewModels.OrderItemStatusChanges;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrderItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> _orderItemStatusChangesModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOrdersRepository _ordersRepository;

        public OrderItemFormModelBuilder
        (
            IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> orderItemStatusChangesModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOrdersRepository ordersRepository)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _ordersRepository = ordersRepository;
            _orderItemStatusChangesModelBuilder = orderItemStatusChangesModelBuilder;
        }

        public async Task<OrderItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _orderLocalizer.GetString("OrderItem"),
                SkuLabel = _globalLocalizer.GetString("Sku"),
                NameLabel = _globalLocalizer.GetString("Name"),
                OrderStatusLabel = _globalLocalizer.GetString("OrderStatus"),
                NavigateToOrderLabel = _orderLocalizer.GetString("NavigateToOrder"),
                OrderStatusCommentLabel = _orderLocalizer.GetString("OrderStatusComment"),
                QuantityLabel = _orderLocalizer.GetString("QuantityLabel"),
                OutletQuantityLabel = _orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = _orderLocalizer.GetString("StockQuantityLabel"),
                DeliveryFromLabel = _orderLocalizer.GetString("DeliveryFrom"),
                DeliveryToLabel = _orderLocalizer.GetString("DeliveryTo"),
                ExternalReferenceLabel = _orderLocalizer.GetString("ExternalReferenceLabel"),
                MoreInfoLabel = _orderLocalizer.GetString("MoreInfoLabel"),
                CancelOrderItemLabel = _orderLocalizer.GetString("CancelOrderItem"),
                CancelOrderItemStatusUrl = _linkGenerator.GetPathByAction("CancelOrderItem", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var orderItem = await _ordersRepository.GetOrderItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (orderItem is not null)
                {
                    viewModel.Id = orderItem.Id;
                    viewModel.ProductName = orderItem.ProductName;
                    viewModel.ProductSku = orderItem.ProductSku;
                    viewModel.OrderItemStatusId = orderItem.OrderItemStatusId;
                    viewModel.Quantity = orderItem.Quantity;
                    viewModel.StockQuantity = orderItem.StockQuantity;
                    viewModel.OrderItemStatusName = orderItem.OrderItemStatusName;
                    viewModel.OutletQuantity = orderItem.OutletQuantity;
                    viewModel.ImageUrl = orderItem.PictureUrl;
                    viewModel.ImageAlt = orderItem.ProductName;
                    viewModel.OrderUrl = _linkGenerator.GetPathByAction("Status", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, id = orderItem.OrderId });
                    viewModel.DeliveryFrom = orderItem.ExpectedDeliveryFrom;
                    viewModel.DeliveryTo = orderItem.ExpectedDeliveryTo;
                    viewModel.ExternalReference = orderItem.ExternalReference;
                    viewModel.MoreInfo = orderItem.MoreInfo;
                    viewModel.CanCancelOrderItem = false;
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
