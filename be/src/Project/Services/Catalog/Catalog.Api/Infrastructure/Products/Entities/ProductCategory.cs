using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Products.Entities
{
    public class ProductCategory : Entity
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
