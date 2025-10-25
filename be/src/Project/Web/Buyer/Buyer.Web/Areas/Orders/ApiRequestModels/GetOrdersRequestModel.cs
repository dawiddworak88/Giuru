using Foundation.ApiExtensions.Models.Request;
using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class GetOrdersRequestModel : PagedRequestModelBase
    {
        public Guid? OrderStatusId { get; set; }
    }
}
