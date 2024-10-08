﻿using Foundation.Catalog.Infrastructure.Categories.Entites;
using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.Catalog.Infrastructure.Categories.Entities
{
    public class Category : Entity
    {
        [Required]
        public int Level { get; set; }

        [Required]
        public int Order { get; set; }

        public Guid? Parentid { get; set; }

        [Required]
        public bool IsLeaf { get; set; }

        [ForeignKey("Parentid")]
        public virtual Category ParentCategory { get; set; }

        public virtual IEnumerable<CategoryImage> Images { get; set; }

        public virtual IEnumerable<CategoryTranslation> Translations { get; set; }

        public virtual IEnumerable<CategorySchema> Schemas { get; set; }
    }
}
