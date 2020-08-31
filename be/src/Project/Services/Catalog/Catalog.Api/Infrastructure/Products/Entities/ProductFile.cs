using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Products.Entities
{
    public class ProductFile : EntityMedia
    {
        public Guid ProductId { get; set; }
    }
}
