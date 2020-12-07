using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Categories.Models
{
    public class CreateCategoryModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
