﻿
using Buyer.Web.Shared.ViewModels.OrderItemStatusChanges;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrderItemFormViewModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderItemStatusId { get; set; }
        public string IdLabel { get; set; }
        public string Title { get; set; }
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string QuantityLabel { get; set; }
        public double StockQuantity { get; set; }
        public string StockQuantityLabel { get; set; }
        public double OutletQuantity { get; set; }
        public string OutletQuantityLabel { get; set; }
        public string OrderStatusLabel { get; set; }
        public string ExpectedDateOfProductOnStockLabel { get; set; }
        public string OrderItemStatusName { get; set; }
        public string ImageAlt { get; set; }
        public string ImageUrl { get; set; }
        public string NavigateToOrderLabel { get; set; }
        public string OrderUrl { get; set; }
        public string ExternalReferenceLabel { get; set; }
        public string ExternalReference { get; set; }
        public string MoreInfoLabel { get; set; }
        public string MoreInfo { get; set; }
        public bool CanCancelOrderItem { get; set; }
        public string CancelOrderItemLabel { get; set; }
        public string CancelOrderItemStatusUrl { get; set; }
        public IEnumerable<OrderItemStatusChangeViewModel> StatusChanges { get; set; }
        public OrderItemStatusChangesViewModel OrderItemStatusChanges { get; set; }
    }
}
