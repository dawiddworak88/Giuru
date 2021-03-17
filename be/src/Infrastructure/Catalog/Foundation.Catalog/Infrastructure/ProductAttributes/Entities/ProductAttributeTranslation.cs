using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Catalog.Infrastructure.ProductAttributes.Entities
{
    public class ProductAttributeTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public Guid ProductAttributeId { get; set; }
    }
}
