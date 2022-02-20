using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class UpdateInventoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
    }
}
