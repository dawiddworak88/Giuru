using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientApprovalRequestModel : RequestModelBase
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
