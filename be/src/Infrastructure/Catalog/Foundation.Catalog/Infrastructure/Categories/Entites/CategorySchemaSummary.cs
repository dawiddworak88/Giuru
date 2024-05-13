using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Catalog.Infrastructure.Categories.Entites
{
    public class CategorySchemaSummary : Entity
    {
        [Required]
        public Guid CategorySchemaId { get; set; }

        [Required]
        public int AttributeCount { get; set; }
    }
}
