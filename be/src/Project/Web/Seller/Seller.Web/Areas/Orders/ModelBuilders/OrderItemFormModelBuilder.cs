using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.Definitions;
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
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> orderItemStatusChangesModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public OrderItemFormModelBuilder
        (
            IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> orderItemStatusChangesModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOrdersRepository ordersRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.ordersRepository = ordersRepository;
            this.orderItemStatusChangesModelBuilder = orderItemStatusChangesModelBuilder;
        }

        public async Task<OrderItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.orderLocalizer.GetString("EditOrderItem"),
                SkuLabel = this.globalLocalizer.GetString("Sku"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                OrderStatusLabel = this.orderLocalizer.GetString("OrderStatus"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NavigateToOrderLabel = this.orderLocalizer.GetString("NavigateToOrder"),
                ExpectedDateOfProductOnStockLabel = this.orderLocalizer.GetString("ExpectedDateOfProductOnStock"),
                SaveUrl = this.linkGenerator.GetPathByAction("Item", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel"),
                ExternalReferenceLabel = this.orderLocalizer.GetString("ExternalReferenceLabel"),
                MoreInfoLabel = this.orderLocalizer.GetString("MoreInfoLabel"),
                CancelOrderItemLabel = this.orderLocalizer.GetString("CancelOrder"),
                CancelOrderItemStatusUrl = this.linkGenerator.GetPathByAction("CancelOrderItem", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var orderStatuses = await this.ordersRepository.GetOrderStatusesAsync(componentModel.Token, componentModel.Language);

                if (orderStatuses is not null)
                {
                    viewModel.OrderItemStatuses = orderStatuses.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
                }

                var orderItem = await this.ordersRepository.GetOrderItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);

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
                    viewModel.OrderUrl = this.linkGenerator.GetPathByAction("Edit", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, id = orderItem.OrderId });
                    viewModel.ExternalReference = orderItem.ExternalReference;
                    viewModel.MoreInfo = orderItem.MoreInfo;
                    viewModel.ExpectedDateOfProductOnStock = orderItem.OrderItemStatusChangeComment;

                    if (orderItem.OrderItemStatusId == OrdersConstants.OrderStatuses.NewId || 
                        orderItem.OrderItemStatusId == Guid.Empty)
                    {
                        viewModel.CanCancelOrderItem = true;
                    }
                }

                if (orderItem.LastOrderItemStatusChangeId is not null)
                {
                    var orderItemStatusChanges = await this.ordersRepository.GetOrderItemStatusesAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                    if (orderItemStatusChanges is not null)
                    {
                        viewModel.StatusChanges = orderItemStatusChanges.StatusChanges.Select(x => new OrderItemStatusChangeViewModel
                        {
                            OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                            OrderItemStatusName = x.OrderItemStatusName,
                            CreatedDate = x.CreatedDate
                        });
                    }

                    viewModel.OrderItemStatusChanges = await this.orderItemStatusChangesModelBuilder.BuildModelAsync(componentModel);
                }
            }
            
            return viewModel;
        }
    }
}
