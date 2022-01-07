using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Catalog.Infrastructure.Products.Entities
{
    public class ProductImage : EntityMedia
    {
        public Guid ProductId { get; set; }
    }
}
