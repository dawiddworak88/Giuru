using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
