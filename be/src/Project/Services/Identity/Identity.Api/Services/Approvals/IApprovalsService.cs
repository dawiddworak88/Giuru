using Foundation.GenericRepository.Paginations;
using Identity.Api.ServicesModels.Approvals;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Services.Approvals
{
    public interface IApprovalsService
    {
        Task<Guid> CreateAsync(CreateApprovalServiceModel model);
        Task<Guid> UpdateAsync(UpdateApprovalServiceModel model);
        Task DeleteAsync(DeleteApprovalServiceModel model);
        Task<ApprovalServiceModel> GetAsync(GetApprovalServiceModel model);
        PagedResults<IEnumerable<ApprovalServiceModel>> Get(GetApprovalsServiceModel model);
    }
}
