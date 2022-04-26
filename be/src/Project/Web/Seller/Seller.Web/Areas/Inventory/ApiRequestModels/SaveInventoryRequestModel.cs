using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Inventory.ApiRequestModels
{
    public class SaveInventoryRequestModel : RequestModelBase
    {
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int Quantity { get; set; }
        public string Ean { get; set; }
        public int? RestockableInDays { get; set; }
        public int? AvailableQuantity { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
