using System;

namespace Seller.Web.Areas.Inventory.ViewModel
{
    public class EditWarehouseFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NameLabel { get; set; }
        public string LocationLabel { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string LocationRequiredErrorMessage { get; set; }
        public string WarehouseUrl { get; set; }
        public string NavigateToWarehouseListText { get; set; }
        public string SaveUrl { get; set; }
        public string IdLabel { get; set; }
        public string SaveText { get; set; }
    }
}
