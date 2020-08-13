using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Products.Entities
{
    public class ProductVideo : EntityMedia
    {
        public Guid ProductId { get; set; }
    }
}
