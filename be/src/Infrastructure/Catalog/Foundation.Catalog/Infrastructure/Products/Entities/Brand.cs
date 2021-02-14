using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Catalog.Infrastructure.Products.Entities
{
    public class Brand : Entity
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
