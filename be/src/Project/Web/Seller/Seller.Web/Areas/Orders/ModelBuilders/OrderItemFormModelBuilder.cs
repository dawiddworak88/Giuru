using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.ViewModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public OrderItemFormModelBuilder
        (
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOrdersRepository ordersRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.ordersRepository = ordersRepository;
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
                OrderStatusCommentLabel = this.orderLocalizer.GetString("OrderStatusComment")
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
                    viewModel.OrderStatusId = orderItem.OrderStatusId;
                    viewModel.OrderUrl = this.linkGenerator.GetPathByAction("Edit", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, id = orderItem.OrderId });
                }
            }
            
            return viewModel;
        }
    }
}
