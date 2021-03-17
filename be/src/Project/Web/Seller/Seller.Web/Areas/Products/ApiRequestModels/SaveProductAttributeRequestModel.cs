using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveProductAttributeRequestModel : RequestModelBase
    {
        public string Name { get; set; }
    }
}
