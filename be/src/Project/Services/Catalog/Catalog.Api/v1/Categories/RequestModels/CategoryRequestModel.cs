using Foundation.ApiExtensions.Models.Request;
using Foundation.Catalog.Infrastructure.Categories.Entites;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public IEnumerable<CategorySchema> Schemas { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
