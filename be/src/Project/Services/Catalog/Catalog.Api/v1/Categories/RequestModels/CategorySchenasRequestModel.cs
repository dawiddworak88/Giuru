using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class CategorySchemasRequestModel
    {
        public Guid Id { get; set; }
        public IEnumerable<CategorySchemaRequestModel> Schemas { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
