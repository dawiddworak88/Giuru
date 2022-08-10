using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Shared.ViewModels.OrderHistory;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.OrderHistory
{
    public class OrderHistoryModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderHistoryViewModel>
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public OrderHistoryModelBuilder(
            IOrdersRepository ordersRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.orderLocalizer = orderLocalizer;
            this.ordersRepository = ordersRepository;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<OrderHistoryViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderHistoryViewModel
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
                    viewModel.OrderStatusesHistory = statusesHistory.StatusesHistory.Select(x => new OrderHistoryItemViewModel
                    {
                        OrderStatusName = x.OrderStatusName,
                        OrderStatusComment = x.OrderStatusComment,
                        CreatedDate = x.CreatedDate
                    });
                }
            }

            return viewModel;
        }
    }
}
