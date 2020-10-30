using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Products.Entities
{
    public class ProductImage : EntityMedia
    {
        public Guid ProductId { get; set; }
    }
}
