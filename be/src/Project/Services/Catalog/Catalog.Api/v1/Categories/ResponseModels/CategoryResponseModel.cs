using System;

namespace Catalog.Api.v1.Categories.ResultModels
{
    public class CategoryResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public bool IsLeaf { get; set; }
        public Guid? ThumbnailMediaId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
