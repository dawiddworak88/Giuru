﻿using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.OrderItemStatusChanges
{
    public class OrderItemStatusChangesViewModel
    {
        public string Title { get; set; }
        public string OrderStatusLabel { get; set; }
        public string OrderStatusCommentLabel { get; set; }
        public string LastModifiedDateLabel { get; set; }
    }
}