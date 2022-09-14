using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ViewModels.OrderItemStatusChanges;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrderItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> orderHistoryModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public OrderItemFormModelBuilder
        (
            IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel> orderHistoryModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOrdersRepository ordersRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.ordersRepository = ordersRepository;
            this.orderHistoryModelBuilder = orderHistoryModelBuilder;
        }

        public async Task<OrderItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.orderLocalizer.GetString("OrderItem"),
                SkuLabel = this.globalLocalizer.GetString("Sku"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                OrderStatusLabel = this.globalLocalizer.GetString("OrderStatus"),
                NavigateToOrderLabel = this.orderLocalizer.GetString("NavigateToOrder"),
                OrderStatusCommentLabel = this.orderLocalizer.GetString("OrderStatusComment"),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel")
            };

            if (componentModel.Id.HasValue)
            {
                var orderItem = await this.ordersRepository.GetOrderItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);

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
                    viewModel.OrderUrl = this.linkGenerator.GetPathByAction("Status", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, id = orderItem.OrderId });
                }

                if (orderItem.LastOrderItemStatusChangeId != Guid.Empty)
                {
                    var componentModelBase = new ComponentModelBase
                    {
                        IsAuthenticated = componentModel.IsAuthenticated,
                        Token = componentModel.Token,
                        Language = componentModel.Language,
                        Id = componentModel.Id
                    };

                    viewModel.OrderItemStatusChanges = await this.orderHistoryModelBuilder.BuildModelAsync(componentModelBase);
                }
            }

            return viewModel;
        }
    }
}
