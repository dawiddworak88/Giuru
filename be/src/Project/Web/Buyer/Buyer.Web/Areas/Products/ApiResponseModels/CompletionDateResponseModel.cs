using System.Collections.Generic;
using Buyer.Web.Areas.Products.DomainModels;
using Foundation.ApiExtensions.Models.Response;
namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class CompletionDateResponseModel : BaseResponseModel
    {
        public List<Product> Products { get; set; }
    }
}
