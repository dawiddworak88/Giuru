using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Outlet.ViewModel
{
    public class NewOutletFormViewModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string OrderName { get; set; }
        public string Title { get; set; }
        public string ProductRequiredErrorMessage { get; set; }
        public string SelectProductLabel { get; set; }
        public string OrderNameLabel { get; set; }
        public string NavigateToOutletListText { get; set; }
        public string OrderNameRequiredErrorMessage { get; set; }
        public string OutletUrl { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public IEnumerable<ListInventoryItemViewModel> Products { get; set; }
    }
}
