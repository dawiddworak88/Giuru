using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientNotificationTypesApprovalsRequestModel
    {
        public Guid? ClientId { get; set; }
        public IEnumerable<Guid> NotificationTypeIds { get; set; }
    }
}
