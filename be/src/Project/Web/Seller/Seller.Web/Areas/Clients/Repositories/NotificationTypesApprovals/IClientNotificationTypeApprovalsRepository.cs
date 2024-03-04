using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Clients.DomainModels;

namespace Seller.Web.Areas.Clients.Repositories.NotificationTypesApprovals
{
    public interface IClientNotificationTypeApprovalsRepository
    {
        Task SaveAsync(string token, string language, Guid? clientId, IEnumerable<Guid> notificationTypeIds);

        Task<IEnumerable<ClientNotificationTypeApproval>> GetAsync(string token, string language, Guid? clientId);
    }
}
