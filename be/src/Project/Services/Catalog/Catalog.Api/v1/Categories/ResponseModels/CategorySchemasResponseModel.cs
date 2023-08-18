using Catalog.Api.ServicesModels.Categories;
using System.Collections.Generic;

namespace Catalog.Api.v1.Categories.ResponseModels
{
    public class CategorySchemasResponseModel
    {
        public IEnumerable<CategorySchemaServiceModel> Schemas { get; set; }        
    }
}
