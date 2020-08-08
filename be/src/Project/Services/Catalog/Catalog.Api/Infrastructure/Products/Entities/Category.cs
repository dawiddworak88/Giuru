using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Products.Entities
{
    public class Category : Entity
    {
        [Required]
        public Guid SchemaId { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public int Order { get; set; }

        public Guid? Parentid { get; set; }

        [Required]
        public bool IsLeaf { get; set; }
    }
}
