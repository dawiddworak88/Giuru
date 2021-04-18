using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Catalog.Infrastructure.Products.Entities
{
    public class ProductTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormData { get; set; }
        public Guid ProductId { get; set; }
    }
}
