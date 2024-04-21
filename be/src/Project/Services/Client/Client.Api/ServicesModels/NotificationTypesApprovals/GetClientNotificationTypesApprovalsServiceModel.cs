using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.NotificationTypesApprovals
{
    public class GetClientNotificationTypesApprovalsServiceModel : BaseServiceModel
    {
        public Guid? ClientId { get; set; }
    }
}
