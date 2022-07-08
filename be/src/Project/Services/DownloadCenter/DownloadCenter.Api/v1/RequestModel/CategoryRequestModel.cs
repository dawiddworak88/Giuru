using System;

namespace DownloadCenter.Api.v1.RequestModel
{
    public class CategoryRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
    }
}
