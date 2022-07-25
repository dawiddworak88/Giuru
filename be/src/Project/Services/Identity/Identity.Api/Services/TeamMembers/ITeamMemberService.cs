using Foundation.GenericRepository.Paginations;
using Identity.Api.ServicesModels.TeamMembers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Services.TeamMembers
{
    public interface ITeamMemberService
    {
        Task CreateAsync(CreateTeamMemberServiceModel model);
        Task UpdateAsync(UpdateTeamMemberServiceModel model);
        Task DeleteAsync(DeleteTeamMemberServiceModel model);
        Task<PagedResults<IEnumerable<TeamMemberServiceModel>>> GetAsync(GetTeamMembersServiceModel model);
        Task<TeamMemberServiceModel> GetAsync(GetTeamMemberServiceModel model);
    }
}
