using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class PagedOrderAttributeOptionsRequestModel : PagedRequestModelBase
    {
        public Guid? AttributeId { get; set; }
    }
}
