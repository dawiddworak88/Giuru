using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class OutletItem : Entity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }

        public virtual IEnumerable<OutletItemTranslation> Translations { get; set; }
    }
}
