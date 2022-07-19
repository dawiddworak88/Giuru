using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.RequestModel
{
    public class CategoryRequestModel : RequestModelBase
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
