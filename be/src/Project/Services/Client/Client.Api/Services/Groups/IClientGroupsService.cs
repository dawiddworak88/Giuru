using Client.Api.ServicesModels.Groups;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Groups
{
    public interface IClientGroupsService
    {
        Task<Guid> CreateAsync(CreateClientGroupServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientGroupServiceModel model);
        Task<ClientGroupServiceModel> GetAsync(GetClientGroupServiceModel model);
        Task<PagedResults<IEnumerable<ClientGroupServiceModel>>> GetAsync(GetClientGroupsServiceModel model);
        Task DeleteAsync(DeleteClientGroupServiceModel model);
        Task<PagedResults<IEnumerable<ClientGroupServiceModel>>> GetByIdsAsync(GetClientGroupsByIdsServiceModel model);
    }
}
