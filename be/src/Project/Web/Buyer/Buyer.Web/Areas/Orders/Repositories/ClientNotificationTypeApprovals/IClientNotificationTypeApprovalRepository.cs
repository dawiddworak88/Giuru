using Buyer.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.ClientNotificationTypeApprovals
{
    public interface IClientNotificationTypeApprovalRepository
    {
        public Task<IEnumerable<ClientNotificationTypeApproval>> GetAsync(string token, string language, Guid? clientId);
    }
}
