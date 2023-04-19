using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
