using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Taxonomies.Entities
{
    public class Taxonomy : Entity
    {
        [Required]
        public int Level { get; set; }

        [Required]
        public int Order { get; set; }

        public Guid? Parentid { get; set; }

        [Required]
        public bool IsLeaf { get; set; }

        public virtual IEnumerable<TaxonomyTranslation> Translations { get; set; }
    }
}
