using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class Warehouse : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public Guid SellerId { get; set; }
    }
}
