using Foundation.ApiExtensions.Models.Request;
using System;

namespace News.Api.v1.Categories.RequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
    }
}
