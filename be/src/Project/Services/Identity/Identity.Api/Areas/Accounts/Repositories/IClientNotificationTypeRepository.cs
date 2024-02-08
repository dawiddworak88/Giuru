using Identity.Api.Areas.Accounts.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.Repositories
{
    public interface IClientNotificationTypeRepository
    {
        public Task<Guid?> SaveAsync(string token, string language, ClientNotificationTypeApprovals model);
        public Task<IEnumerable<ClientNotificationTypeApproval>> GetByIds(string token, string language, string ids, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
