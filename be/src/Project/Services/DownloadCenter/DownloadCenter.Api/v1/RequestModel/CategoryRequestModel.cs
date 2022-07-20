using Foundation.ApiExtensions.Models.Request;
using System;

namespace DownloadCenter.Api.v1.RequestModel
{
    public class CategoryRequestModel : RequestModelBase
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
    }
}
