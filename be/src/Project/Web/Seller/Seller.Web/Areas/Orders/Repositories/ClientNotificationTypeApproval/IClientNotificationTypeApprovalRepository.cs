using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.Repositories.ClientNotificationTypeApproval
{
    public interface IClientNotificationTypeApprovalRepository
    {
        public Task<IEnumerable<DomainModels.ClientNotificationTypeApproval>> GetAsync(string token, string language, Guid? clientId);
    }
}
