using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.Notification
{
    public class SaveNotificationTypeApprovalServiceModel : BaseServiceModel
    {
        public Guid ClientId { get; set; }
        public IEnumerable<Guid> NotificationTypeIds { get; set; }
    }
}
