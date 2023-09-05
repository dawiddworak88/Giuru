using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Categories
{
    public class CreateCategoryServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public IEnumerable<CategorySchemaServiceModel> Schemas { get; set; }
        public Guid? ParentId { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
