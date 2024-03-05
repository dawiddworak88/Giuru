using Client.Api.ServicesModels.NotificationTypesApprovals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.NotificationTypesApprovals
{
    public interface IClientNotificationTypesApprovalsService
    {
        IEnumerable<ClientNotificationTypeApprovalServiceModel> Get(GetClientNotificationTypesApprovalsServiceModel model);
        Task SaveAsync(SaveNotificationTypesApprovalsServiceModel model);
    }
}
