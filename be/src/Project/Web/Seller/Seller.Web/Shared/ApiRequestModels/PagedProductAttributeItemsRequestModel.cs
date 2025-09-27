using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class PagedProductAttributeItemsRequestModel : PagedRequestModelBase
    {
        public Guid? ProductAttributeId { get; set; }
    }
}
