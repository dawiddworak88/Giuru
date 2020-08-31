using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Taxonomies.Entities
{
    public class TaxonomyTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public Guid TaxonomyId { get; set; }
    }
}
