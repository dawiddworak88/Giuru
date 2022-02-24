using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.News.ApiRequestModels
{
    public class CreateCategoryRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
