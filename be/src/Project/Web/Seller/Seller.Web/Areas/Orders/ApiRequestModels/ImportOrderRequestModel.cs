using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;
using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class ImportOrderRequestModel : RequestModelBase
    {
        public Guid? ClientId { get; set; }
        public IFormFile OrderFile { get; set; }
    }
}
