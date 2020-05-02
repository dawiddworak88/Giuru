using Foundation.ApiExtensions.Models.Request;
using System;

namespace Tenant.Portal.Areas.Clients.ApiRequestModels
{
    public class ClientRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
    }
}
