using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;
using System;

namespace Foundation.Catalog.Infrastructure.Products.Entities
{
    public class ProductsGroup : Entity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
