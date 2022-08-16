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
                OrderStatusLabel = this.orderLocalizer.GetString("OrderStatus"),
                OrderStatusCommentLabel = this.orderLocalizer.GetString("OrderStatusComment"),
                LastModifiedDateLabel = this.globalLocalizer.GetString("LastModifiedDate")
            };

            if (componentModel.Id.HasValue)
            {
                var statusesHistory = await this.ordersRepository.GetOrderItemStatusesAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (statusesHistory is not null)
                {
                    viewModel.OrderStatusesHistory = statusesHistory.StatusesHistory.Select(x => new OrderItemStatusChangeViewModel
                    {
                        OrderItemStatusName = x.OrderStatusName,
                        OrderItemStatusChangeComment = x.OrderStatusComment,
                        CreatedDate = x.CreatedDate
                    });
                }
            }

            return viewModel;
        }
    }
}
