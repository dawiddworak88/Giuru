using Foundation.Catalog.Infrastructure.Categories.Entites;
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
        public IEnumerable<CategorySchema> Schemas { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
