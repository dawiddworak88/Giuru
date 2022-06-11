using Client.Api.ServicesModels.Managers;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Managers
{
    public interface IClientAccountManagersService
    {
        Task<Guid> CreateAsync(CreateClientAccountManagerServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientAccountManagerServiceModel model);
        Task<ClientAccountManagerServiceModel> GetAsync(GetClientAccountManagerServiceModel model);
        Task<PagedResults<IEnumerable<ClientAccountManagerServiceModel>>> GetAsync(GetClientAccountManagersServiceModel model);
        Task DeleteAsync(DeleteClientAccountManagerServiceModel model);
        Task<PagedResults<IEnumerable<ClientAccountManagerServiceModel>>> GetByIdsAsync(GetClientAccountManagersByIdsServiceModel model);
    }
}
