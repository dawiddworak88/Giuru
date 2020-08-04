using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class ProductSchemaRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
    }
}
