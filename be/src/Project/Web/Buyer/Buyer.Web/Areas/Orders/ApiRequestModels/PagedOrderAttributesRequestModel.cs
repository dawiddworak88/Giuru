using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class PagedOrderAttributesRequestModel : PagedRequestModelBase
    {
        public bool? ForOrderItems { get; set; }
    }
}
