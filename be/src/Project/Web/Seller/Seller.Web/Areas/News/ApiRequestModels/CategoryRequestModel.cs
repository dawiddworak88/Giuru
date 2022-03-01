using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.News.ApiRequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
