using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Tenant.Portal.Areas.Products.ApiResponseModels
{
    public class ProductsResponseModel : BaseResponseModel
    {
        public PagedResults<IEnumerable<ProductResponseModel>> Products { get; set; }
    }
}
