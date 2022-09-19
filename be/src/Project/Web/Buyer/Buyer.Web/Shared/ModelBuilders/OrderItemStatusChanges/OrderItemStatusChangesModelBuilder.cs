using Buyer.Web.Shared.ViewModels.OrderItemStatusChanges;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.OrderItemStatusChanges
{
    public class OrderItemStatusChangesModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel>
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public OrderItemStatusChangesModelBuilder(
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.orderLocalizer = orderLocalizer;
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

            return viewModel;
        }
    }
}
