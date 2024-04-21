using System;
using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.ApiRequestModels
{
    public class SaveClientNotificationTypaApprovalRequestModel
    {
        public Guid ClientId { get; set; }
        public IEnumerable<Guid> NotificationTypeIds { get; set; }
    }
}
