using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class PagedOrderAttributeValuesRequestModel : PagedRequestModelBase
    {
        public Guid? OrderId { get; set; }
    }
}
