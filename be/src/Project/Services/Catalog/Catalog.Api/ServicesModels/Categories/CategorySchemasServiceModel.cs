using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Categories
{
    public class CategorySchemasServiceModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<CategorySchemaServiceModel> Schemas { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
