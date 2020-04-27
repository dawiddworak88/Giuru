using System;

namespace Tenant.Portal.Areas.Products.ApiRequestModels
{
    public class ProductRequestModel
    {
        public Guid? Id { get; set; }
        public string Sku { get; set; }
    }
}
