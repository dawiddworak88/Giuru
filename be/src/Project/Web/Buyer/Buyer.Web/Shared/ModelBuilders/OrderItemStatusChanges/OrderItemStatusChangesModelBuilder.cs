using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Shared.ViewModels.OrderItemStatusChanges;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.OrderItemStatusChanges
{
    public class OrderItemStatusChangesModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel>
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public OrderItemStatusChangesModelBuilder(
            IOrdersRepository ordersRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.orderLocalizer = orderLocalizer;
            this.ordersRepository = ordersRepository;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<OrderItemStatusChangesViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemStatusChangesViewModel
            {
                Title = this.orderLocalizer.GetString("HistoryOfStatusChanges"),
                OrderStatusLabel = this.globalLocalizer.GetString("OrderStatus"),
                OrderStatusCommentLabel = this.globalLocalizer.GetString("OrderStatusComment"),
                LastModifiedDateLabel = this.globalLocalizer.GetString("LastModifiedDate")
            };

            if (componentModel.Id.HasValue)
            {
                var orderItemChanges = await this.ordersRepository.GetOrderItemStatusesAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (orderItemChanges is not null)
                {
                    viewModel.OrderItemStatusChanges = orderItemChanges.StatusChanges.Select(x => new OrderItemStatusChangeViewModel
                    {
                        OrderItemStatusName = x.OrderItemStatusName,
                        OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                        CreatedDate = x.CreatedDate
                    });
                }
            }

            return viewModel;
        }
    }
}
