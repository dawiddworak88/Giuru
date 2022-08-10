using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.OrderHistory
{
    public class OrderHistoryViewModel
    {
        public string Title { get; set; }
        public string OrderStatusLabel { get; set; }
        public string OrderStatusCommentLabel { get; set; }
        public string LastModifiedDateLabel { get; set; }
        public IEnumerable<OrderHistoryItemViewModel> OrderStatusesHistory { get; set; }
    }
}
