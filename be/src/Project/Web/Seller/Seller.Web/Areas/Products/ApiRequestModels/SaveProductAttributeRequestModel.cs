using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveProductAttributeRequestModel : RequestModelBase
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
