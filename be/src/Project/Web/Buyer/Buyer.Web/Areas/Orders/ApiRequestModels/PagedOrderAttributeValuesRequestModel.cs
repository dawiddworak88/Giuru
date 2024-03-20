using Foundation.ApiExtensions.Models.Request;
using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class PagedOrderAttributeValuesRequestModel : PagedRequestModelBase
    {
        public Guid? OrderId { get; set; }
    }
}
