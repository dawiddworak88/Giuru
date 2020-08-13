using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Brands.Entities
{
    public class BrandVideo : EntityMedia
    {
        public Guid BrandId { get; set; }
    }
}
