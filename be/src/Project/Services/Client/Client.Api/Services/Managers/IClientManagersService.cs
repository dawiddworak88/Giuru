using Client.Api.ServicesModels.Managers;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Managers
{
    public interface IClientManagersService
    {
        Task<Guid> CreateAsync(CreateClientManagerServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientManagerServiceModel model);
        Task<ClientManagerServiceModel> GetAsync(GetClientManagerServiceModel model);
        Task<PagedResults<IEnumerable<ClientManagerServiceModel>>> GetAsync(GetClientManagersServiceModel model);
        Task DeleteAsync(DeleteClientManagerServiceModel model);
        Task<PagedResults<IEnumerable<ClientManagerServiceModel>>> GetByIdsAsync(GetClientManagersByIdsServiceModel model);
    }
}
