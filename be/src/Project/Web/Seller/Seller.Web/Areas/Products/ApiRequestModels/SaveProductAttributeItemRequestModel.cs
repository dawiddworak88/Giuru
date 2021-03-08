using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveProductAttributeItemRequestModel : RequestModelBase
    {
        public Guid? ProductAttributeId { get; set; }
        public string Name { get; set; }
    }
}
