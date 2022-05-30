using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class InventoryItem : Entity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double AvailableQuantity { get; set; }

        public int? RestockableInDays { get; set; }

        public DateTime? ExpectedDelivery { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }
    }
}
