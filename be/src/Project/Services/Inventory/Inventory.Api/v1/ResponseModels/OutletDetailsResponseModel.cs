using System;

namespace Inventory.Api.v1.ResponseModels
{
    public class OutletDetailsResponseModel
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public double Quantity { get; set; }
        public string Ean { get; set; }
        public double? AvailableQuantity { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
