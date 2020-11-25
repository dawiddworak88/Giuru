using System;

namespace Catalog.Api.v1.Areas.Categories.ResultModels
{
    public class CategoryResultModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SchemaId { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public bool IsLeaf { get; set; }
        public Guid? ThumbnailMediaId { get; set; }
    }
}
