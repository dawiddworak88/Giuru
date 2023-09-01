using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Categories
{
    public class UpdateCategoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public int Order { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
