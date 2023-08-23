using Foundation.ApiExtensions.Models.Request;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveCategoryApiRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public IEnumerable<CategorySchema> Schemas { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
