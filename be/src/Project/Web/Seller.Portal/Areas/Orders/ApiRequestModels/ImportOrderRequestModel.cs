using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;
using System;

namespace Seller.Portal.Areas.Orders.ApiRequestModels
{
    public class ImportOrderRequestModel : BaseRequestModel
    {
        public Guid? ClientId { get; set; }
        public IFormFile OrderFile { get; set; }
    }
}
