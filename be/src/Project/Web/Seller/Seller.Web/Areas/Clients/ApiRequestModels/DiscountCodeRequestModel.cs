using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class DiscountCodeRequestModel : RequestModelBase
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
    }
}
