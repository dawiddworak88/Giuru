using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class ProductsResponseModel : BaseResponseModel
    {
        public PagedResults<IEnumerable<ProductResponseModel>> PagedProducts { get; set; }
    }
}
