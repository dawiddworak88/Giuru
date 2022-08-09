using Seller.Web.Areas.Orders.DomainModels;
using System.Collections.Generic;

namespace Seller.Web.Shared.ViewModels
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
