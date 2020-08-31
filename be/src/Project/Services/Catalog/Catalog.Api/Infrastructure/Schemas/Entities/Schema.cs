using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Catalog.Api.Infrastructure.Schemas.Entities
{
    public class Schema : Entity
    {
        public virtual IEnumerable<SchemaTranslation> Translations { get; set; }
    }
}
