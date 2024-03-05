using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.NotificationTypesApprovals
{
    public class SaveNotificationTypesApprovalsServiceModel : BaseServiceModel
    {
        public Guid ClientId { get; set; }
        public IEnumerable<Guid> NotificationTypeIds { get; set; }
    }
}
