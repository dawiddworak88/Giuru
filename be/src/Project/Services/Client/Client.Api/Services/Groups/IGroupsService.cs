using Client.Api.ServicesModels.Groups;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Groups
{
    public interface IGroupsService
    {
        Task<Guid> CreateAsync(CreateGroupServiceModel model);
        Task<Guid> UpdateAsync(UpdateGroupServiceModel model);
        Task<GroupServiceModel> GetAsync(GetGroupServiceModel model);
        Task<PagedResults<IEnumerable<GroupServiceModel>>> GetAsync(GetGroupsServiceModel model);
        Task DeleteAsync(DeleteGroupServiceModel model);
    }
}
