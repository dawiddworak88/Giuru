using Foundation.ApiExtensions.Models.Response;

namespace Api.v1.Areas.Products.ResponseModels
{
    public class ProductResponseModel : BaseResponseModel
    {
        public Foundation.TenantDatabase.Areas.Products.Entities.Product Product { get; set; }
    }
}
