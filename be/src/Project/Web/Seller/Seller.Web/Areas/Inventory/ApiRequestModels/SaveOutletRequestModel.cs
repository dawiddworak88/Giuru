using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Inventory.ApiRequestModels
{
    public class SaveOutletRequestModel : RequestModelBase
    {
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
