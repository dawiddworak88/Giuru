using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Products.Entities
{
    public class Product : Entity
    {
        [Required]
        public string Sku { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid SchemaId { get; set; }

        public string FormData { get; set; }

        [Required]
        public Guid ProductTypeId { get; set; }

        [Required]
        public Guid SellerId { get; set; }
    }
}
