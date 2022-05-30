using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Inventory.ViewModel
{
    public class OutletFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseRequiredErrorMessage { get; set; }
        public string ProductRequiredErrorMessage { get; set; }
        public string QuantityRequiredErrorMessage { get; set; }
        public string QuantityFormatErrorMessage { get; set; }
        public string SelectWarehouseLabel { get; set; }
        public string SelectProductLabel { get; set; }
        public string QuantityLabel { get; set; }
        public string AvailableQuantityLabel { get; set; }
        public string OkLabel { get; set; }
        public string CancelLabel { get; set; }
        public string OutletUrl { get; set; }
        public string SelectWarehouse { get; set; }
        public string NavigateToOutletListText { get; set; }
        public IEnumerable<ListItemViewModel> Warehouses { get; set; }
        public IEnumerable<ListOutletItemViewModel> Products { get; set; }
        public double Quantity { get; set; }
        public double AvailableQuantity { get; set; }
        public string SaveUrl { get; set; }
        public string IdLabel { get; set; }
        public string SaveText { get; set; }
        public string OutletTitle { get; set; }
        public string TitleLabel { get; set; }
        public string OutletDescription { get; set; }
        public string DescriptionLabel { get; set; }
        public string EanLabel { get; set; }
        public string Ean { get; set; }
        public string ProductsSuggestionUrl { get; set; }
        public ListOutletItemViewModel Product { get; set; }
    }
}
