using Buyer.Web.Shared.ViewModels.Files;
using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class StatusOrderFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public Guid OrderStatusId { get; set; }
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string QuantityLabel { get; set; }
        public string StockQuantityLabel { get; set; }
        public string OutletQuantityLabel { get; set; }
        public string ExternalReferenceLabel { get; set; }
        public string DeliveryFromLabel { get; set; }
        public string DeliveryToLabel { get; set; }
        public string ExpectedDeliveryLabel { get; set; }
        public string FabricsLabel { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public string MoreInfoLabel { get; set; }
        public string OrderItemsLabel { get; set; }
        public string OrderStatusLabel { get; set; }
        public bool CanCancelOrder { get; set; }
        public string CancelOrderLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string UpdateOrderStatusUrl { get; set; }
        public string OrderStatusCommentLabel { get; set; }
        public string CustomOrder { get; set; }
        public string CustomOrderLabel { get; set; }
        public string EditUrl { get; set; }
        public string YesLabel { get; set; }
        public string NoLabel { get; set; }
        public string CancelationConfirmationDialogLabel { get; set; }
        public string AreYouSureToCancelOrderLabel { get; set; }
        public IEnumerable<ListItemViewModel> OrderStatuses { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
        public FilesViewModel Attachments { get; set; }
    }
}
