using Identity.Api.ServicesModels.UserApprovals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Services.UserApprovals
{
    public interface IUserApprovalsService
    {
        IEnumerable<UserApprovalServiceModel> Get(GetUserApprovalsServiceModel model);
        Task SaveAsync(SaveUserApprovalsServiceModel model);
    }
}
