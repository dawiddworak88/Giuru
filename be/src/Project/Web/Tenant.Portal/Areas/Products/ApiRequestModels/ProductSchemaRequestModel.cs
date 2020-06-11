using Foundation.ApiExtensions.Models.Request;
using System;

namespace Tenant.Portal.Areas.Products.ApiRequestModels
{
    public class ProductSchemaRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
    }
}
