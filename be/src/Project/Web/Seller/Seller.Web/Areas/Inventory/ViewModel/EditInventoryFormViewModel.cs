using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Inventory.ViewModel
{
    public class EditInventoryFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public string WarehouseRequiredErrorMessage { get; set; }
        public string ProductRequiredErrorMessage { get; set; }
        public string QuantityRequiredErrorMessage { get; set; }
        public string QuantityFormatErrorMessage { get; set; }
        public string SelectWarehouseLabel { get; set; }
        public string SelectProductLabel { get; set; }
        public string QuantityLabel { get; set; }
        public string AvailableQuantityLabel { get; set; }
        public string RestockableInDaysLabel { get; set; }
        public string ExpectedDeliveryLabel { get; set; }
        public string OkLabel { get; set; }
        public string CancelLabel { get; set; }
        public string ChangeExpectedDeliveryLabel { get; set; }
        public string InventoryUrl { get; set; }
        public string SelectWarehouse { get; set; }
        public string NavigateToInventoryListText { get; set; }
        public IEnumerable<ListItemViewModel> Warehouses { get; set; }
        public IEnumerable<ListInventoryItemViewModel> Products { get; set; }
        public int? RestockableInDays { get; set; }
        public int Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string IdLabel { get; set; }
    }
}
