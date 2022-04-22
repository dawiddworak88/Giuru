using System;
namespace Inventory.Api.Infrastructure.Entities
{
    public class InventoryItem : InventoryProduct
    {

        public int? RestockableInDays { get; set; }

        public DateTime? ExpectedDelivery { get; set; }
    }
}
