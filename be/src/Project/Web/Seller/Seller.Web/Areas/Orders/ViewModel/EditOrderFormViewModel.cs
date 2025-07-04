﻿using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class EditOrderFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public Guid OrderStatusId { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string SaveText { get; set; }
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string QuantityLabel { get; set; }
        public string StockQuantityLabel { get; set; }
        public string OutletQuantityLabel { get; set; }
        public string InTotalLabel { get; set; }
        public string ExternalReferenceLabel { get; set; }
        public string MoreInfoLabel { get; set; }
        public string OrderItemsLabel { get; set; }
        public string OrderStatusLabel { get; set; }
        public string ClientLabel { get; set; }
        public string ClientUrl { get; set; }
        public string ClientName { get; set; }
        public string UpdateOrderStatusUrl { get; set; }
        public string IdLabel { get; set; }
        public bool CanCancelOrder { get; set; }
        public string CancelOrderLabel { get; set; }
        public string CancelOrderStatusUrl { get; set; }
        public string CustomOrderLabel { get; set; }
        public string CustomOrder { get; set; }
        public string UpdateOrderItemStatusUrl { get; set; }
        public string EditUrl { get; set; }
        public string ExpectedDateOfProductOnStockLabel { get; set; }
        public string OrdersUrl { get; set; }
        public string NavigateToOrders { get; set; }
        public string DeliveryAddressLabel { get; set; }
        public string BillingAddressLabel { get; set; }
        public string DeliveryAddress { get; set; }
        public string BillingAddress { get; set; }
        public string SearchTerm { get; set; }
        public int? MaxAllowedOrderQuantity { get; set; }
        public string MaxAllowedOrderQuantityErrorMessage { get; set; }
        public FilesViewModel Attachments { get; set; }
        public IEnumerable<ListItemViewModel> OrderStatuses { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
        public IEnumerable<OrderItemStatusViewModel> OrderItemsStatuses { get; set; }
    }
}
