using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Catalog.Infrastructure.Categories.Entites
{
    public class CategoryTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
    }
}
