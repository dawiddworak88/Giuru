using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class UpdateMediaItemVersionRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
