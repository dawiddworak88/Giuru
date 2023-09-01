using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveCategoryRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int Order { get; set; }
        public IEnumerable<FileRequestModel> Files { get; set; }
    }
}
