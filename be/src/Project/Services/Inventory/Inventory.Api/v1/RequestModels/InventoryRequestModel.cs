using Foundation.ApiExtensions.Models.Request;
using System;

namespace Inventory.Api.v1.RequestModels
{
    public class InventoryRequestModel : RequestModelBase
    {
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public double Quantity { get; set; }
        public string Ean { get; set; }
        public double AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
