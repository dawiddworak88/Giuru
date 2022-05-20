using Foundation.PageContent.Components.ListItems.ViewModels;
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
        public string ExternalReferenceLabel { get; set; }
        public string DeliveryFromLabel { get; set; }
        public string DeliveryToLabel { get; set; }
        public string MoreInfoLabel { get; set; }
        public string OrderItemsLabel { get; set; }
        public string OrderStatusLabel { get; set; }
        public string ClientLabel { get; set; }
        public string ClientUrl { get; set; }
        public string ClientName { get; set; }
        public string UpdateOrderStatusUrl { get; set; }
        public string IdLabel { get; set; }
        public string CustomOrderLabel { get; set; }
        public string CustomOrder { get; set; }
        public IEnumerable<ListItemViewModel> OrderStatuses { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
        public FilesViewModel Attachments { get; set; }
    }
}
