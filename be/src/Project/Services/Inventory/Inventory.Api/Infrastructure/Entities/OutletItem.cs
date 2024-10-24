﻿using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Api.Infrastructure.Entities
{
    public class OutletItem : Entity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double AvailableQuantity { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        public virtual IEnumerable<OutletItemTranslation> Translations { get; set; }
    }
}
