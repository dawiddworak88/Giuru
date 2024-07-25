using Buyer.Web.Areas.Products.DomainModels;
using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class CompletionDateRespopnseModel : BaseResponseModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
