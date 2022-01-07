using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class PagedProductAttributeItemsRequestModel : PagedRequestModelBase
    {
        public Guid? ProductAttributeId { get; set; }
    }
}
