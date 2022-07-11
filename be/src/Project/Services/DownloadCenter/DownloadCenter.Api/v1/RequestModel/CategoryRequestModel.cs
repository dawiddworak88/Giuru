using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.RequestModel
{
    public class CategoryRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
