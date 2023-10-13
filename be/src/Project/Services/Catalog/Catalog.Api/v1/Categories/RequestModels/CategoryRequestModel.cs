using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public IEnumerable<CategorySchemaRequestModel> Schemas { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int Order { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
