using Foundation.GenericRepository.Paginations;
using Identity.Api.ServicesModels.ClientTeamMembers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Services.ClientTeamMembers
{
    public interface IClientTeamMemberService
    {
        Task<Guid> CreateAsync(CreateClientTeamMemberServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientTeamMemberServiceModel model);
        Task DeleteAsync(DeleteClientTeamMemberServiceModel model);
        Task<PagedResults<IEnumerable<ClientTeamMemberServiceModel>>> GetAsync(GetClientTeamMembersServiceModel model);
        Task<ClientTeamMemberServiceModel> GetAsync(GetClientTeamMemberServiceModel model);
    }
}
