using Foundation.ApiExtensions.Models.Response;
using System;

namespace Seller.Web.Areas.Clients.ApiResponseModels
{
    public class ClientResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
