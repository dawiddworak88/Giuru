using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderItemFormViewModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderStatusId { get; set; }
        public string IdLabel { get; set; }
        public string Title { get; set; }
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string OrderStatusLabel { get; set; }
        public string OrderStatusCommentLabel { get; set; }
        public string NavigateToOrderLabel { get; set; }
        public string SaveText { get; set; }
        public string OrderUrl { get; set; }
        public string SaveUrl { get; set; }
        public OrderHistoryViewModel OrderStatusesHistory { get; set; }
        public IEnumerable<ListItemViewModel> OrderItemStatuses { get; set; }
    }
}
