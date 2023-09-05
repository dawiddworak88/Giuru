using Catalog.Api.ServicesModels.Categories;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Categories.ResponseModels
{
    public class CategorySchemasResponseModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<CategorySchemaResponseModel> Schemas { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
