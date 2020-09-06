using Foundation.ApiExtensions.Models.Request;
using System;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class CategoryRequestModel : RequestModelBase
    {
        public Guid? Id { get; set; }
    }
}
