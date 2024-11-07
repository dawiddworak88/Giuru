using Buyer.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.UserApprovals
{
    public interface IUserApprovalsRepository
    {
        Task<IEnumerable<UserApproval>> GetAsync(string token, string language, Guid id);
    }
}
