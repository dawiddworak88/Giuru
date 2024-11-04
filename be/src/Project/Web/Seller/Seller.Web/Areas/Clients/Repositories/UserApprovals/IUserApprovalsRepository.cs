using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.UserApprovals
{
    public interface IUserApprovalsRepository
    {
        Task<IEnumerable<UserApproval>> GetAsync(string token, string language, Guid? id);
        Task SaveAsync(string token, string language, IEnumerable<Guid> userApprovals, Guid userId);
    }
}
