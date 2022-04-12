using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiResponseModels
{
    public class ProductsResponseModel : BaseResponseModel
    {
        public PagedResults<IEnumerable<ProductResponseModel>> PagedProducts { get; set; }
    }
}