using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Products.Entities
{
    public class CategoryProduct : Entity
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
