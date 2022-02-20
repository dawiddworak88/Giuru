using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class InventoryItem : Entity
    {
        [Required]
        public Guid WarehouseId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductSku { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        public int? RestockableInDays { get; set; }

        public DateTime? ExpectedDelivery { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }
}
