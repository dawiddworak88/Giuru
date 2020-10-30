using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Schemas.Entities
{
    public class SchemaTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public string JsonSchema { get; set; }
        public Guid SchemaId { get; set; }
    }
}
