using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.NotificationTypeApproval
{
    public interface IClientNotificationTypeApproval
    {
        public Task<IEnumerable<DomainModels.ClientNotificationTypeApproval>> GetAsync(string token, string language, Guid? clientId);
    }
}
