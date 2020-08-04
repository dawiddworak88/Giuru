using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class DeleteProductRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
    }
}
