using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Brands.Entities
{
    public class BrandFile : EntityMedia
    {
        public Guid BrandId { get; set; }
    }
}
