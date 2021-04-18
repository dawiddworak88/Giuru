using Foundation.ApiExtensions.Models.Request;

namespace Catalog.Api.v1.Products.RequestModels
{
    public class ProductAttributeRequestModel : RequestModelBase
    {
        public string Name { get; set; }
    }
}
