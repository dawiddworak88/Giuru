using Foundation.ApiExtensions.Models.Request;

namespace Api.v1.Areas.Products.RequestModels
{
    public class ProductRequestModel : BaseRequestModel
    {
        public string Sku { get; set; }
        public string Name { get; set; }
    }
}
