using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace News.Api.Infrastructure.Entities
{
    public class Category : Entity
    {
        public Guid? ParentCategoryId { get; set; }
        public virtual IEnumerable<CategoryTranslation> Translations { get; set; }
    }
}
