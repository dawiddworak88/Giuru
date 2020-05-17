using Foundation.ApiExtensions.Models.Request;
using System;

namespace Tenant.Portal.Areas.Products.ApiRequestModels
{
    public class ProductRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
    }
}
