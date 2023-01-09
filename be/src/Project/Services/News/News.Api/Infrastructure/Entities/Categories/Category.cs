using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Api.Infrastructure.Entities.Categories
{
    public class Category : Entity
    {
        public Guid? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; }

        public virtual IEnumerable<CategoryTranslation> Translations { get; set; }
    }
}
