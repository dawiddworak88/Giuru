using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Catalog.Infrastructure.ProductAttributes.Entities
{
    public class ProductAttributeItemTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public Guid ProductAttributeItemId { get; set; }
    }
}
