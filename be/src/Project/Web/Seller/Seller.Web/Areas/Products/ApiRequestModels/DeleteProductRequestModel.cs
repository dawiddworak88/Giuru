using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class DeleteProductRequestModel : RequestModelBase
    {
        public Guid? Id { get; set; }
    }
}
