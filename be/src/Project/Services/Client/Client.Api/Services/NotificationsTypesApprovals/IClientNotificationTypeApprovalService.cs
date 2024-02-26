using Client.Api.ServicesModels.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.NotificationsTypesApprovals
{
    public interface IClientNotificationTypeApprovalService
    {
        IEnumerable<ClientNotificationTypeApprovalServiceModel> Get(GetClientNotificationTypeApprovalsServiceModel model);
        Task SaveAsync(SaveNotificationTypeApprovalServiceModel model);
    }
}
