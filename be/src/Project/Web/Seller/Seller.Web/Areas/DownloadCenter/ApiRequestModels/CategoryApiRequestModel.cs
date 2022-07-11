using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.ApiRequestModels
{
    public class CategoryApiRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public bool IsVisible { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
