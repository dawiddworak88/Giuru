using Foundation.GenericRepository.Paginations;
using Identity.Api.ServicesModels.TeamMembers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Services.TeamMembers
{
    public interface ITeamMemberService
    {
        Task<Guid> CreateAsync(CreateTeamMemberServiceModel model);
        Task<Guid> UpdateAsync(UpdateTeamMemberServiceModel model);
        Task DeleteAsync(DeleteTeamMemberServiceModel model);
        Task<PagedResults<IEnumerable<TeamMemberServiceModel>>> GetAsync(GetTeamMembersServiceModel model);
        Task<TeamMemberServiceModel> GetAsync(GetTeamMemberServiceModel model);
    }
}
