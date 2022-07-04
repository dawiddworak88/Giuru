using System;

namespace Download.Api.v1.Categories.RequestModel
{
    public class CategoryRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
    }
}
