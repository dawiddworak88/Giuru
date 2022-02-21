using System;

namespace Inventory.Api.v1.ResponseModels
{
    public class OutletDetailsResponseModel
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
