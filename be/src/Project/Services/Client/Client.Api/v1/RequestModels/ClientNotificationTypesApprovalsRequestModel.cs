using System;
using System.Collections.Generic;

namespace Client.Api.v1.RequestModels
{
    public class ClientNotificationTypesApprovalsRequestModel
    {
        public Guid ClientId { get; set; }
        public IEnumerable<Guid> NotificationTypeIds { get; set; }
    }
}
