using Client.Api.ServicesModels.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.NotificationsType
{
    public interface IClientNotificationTypeApprovalService
    {
        IEnumerable<ClientNotificationTypeApprovalServiceModel> Get(GetClientNotificationTypeApprovalsServiceModel model);
        Task SaveAsync(SaveNotificationTypeApprovalServiceModel model);
    }
}
