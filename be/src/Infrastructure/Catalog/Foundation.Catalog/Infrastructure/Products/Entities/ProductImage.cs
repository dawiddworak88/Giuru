using Foundation.GenericRepository.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Foundation.Catalog.Infrastructure.Products.Entities
{
    [Index(nameof(ProductId))]
    public class ProductImage : EntityMedia
    {
        public Guid ProductId { get; set; }
    }
}
