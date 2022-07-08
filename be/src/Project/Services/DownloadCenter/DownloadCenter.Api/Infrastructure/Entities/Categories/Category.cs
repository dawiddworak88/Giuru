using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.Infrastructure.Entities.Categories
{
    public class Category : Entity
    {
        public Guid? ParentCategoryId { get; set; }
        public bool IsVisible { get; set; }
        public virtual IEnumerable<CategoryTranslation> Translations { get; set; }
    }
}