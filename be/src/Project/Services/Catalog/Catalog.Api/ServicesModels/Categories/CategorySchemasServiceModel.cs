using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Categories
{
    public class CategorySchemasServiceModel
    {
        public Guid? CategoryId { get; set; }
        public IEnumerable<SchemaServiceModel> Schemas { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
