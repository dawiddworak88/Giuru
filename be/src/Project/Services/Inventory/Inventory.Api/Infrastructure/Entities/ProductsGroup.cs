using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class ProductsGroup : Entity 
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
