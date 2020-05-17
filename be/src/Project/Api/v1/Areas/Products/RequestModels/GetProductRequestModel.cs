using Foundation.ApiExtensions.Models.Request;
using System;

namespace Api.v1.Areas.Products.RequestModels
{
    public class GetProductRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
    }
}
