using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.DownloadCenter.ApiRequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
