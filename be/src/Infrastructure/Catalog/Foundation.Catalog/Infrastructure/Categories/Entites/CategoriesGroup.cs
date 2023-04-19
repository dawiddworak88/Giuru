using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;
using System;

namespace Foundation.Catalog.Infrastructure.Categories.Entites
{
    public class CategoriesGroup : Entity
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
