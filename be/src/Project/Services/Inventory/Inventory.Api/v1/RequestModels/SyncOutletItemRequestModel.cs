using System;

namespace Inventory.Api.v1.RequestModels
{
    public class SyncOutletItemRequestModel
    {
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public double Quantity { get; set; }
        public double? AvailableQuantity { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
