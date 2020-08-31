using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Taxonomies.Entities
{
    public class TaxonomyImage : EntityMedia
    {
        public Guid TaxonomyId { get; set; }
    }
}
