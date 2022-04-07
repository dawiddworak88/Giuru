using Client.Api.ServicesModels;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services
{
    public interface IClientsService
    {
        Task<PagedResults<IEnumerable<ClientServiceModel>>> GetAsync(GetClientsServiceModel model);
        Task<ClientServiceModel> GetAsync(GetClientServiceModel model);
        Task<ClientServiceModel> GetByOrganisationAsync(GetClientByOrganisationServiceModel model);
        Task DeleteAsync(DeleteClientServiceModel model);
        Task<ClientServiceModel> UpdateAsync(UpdateClientServiceModel serviceModel);
        Task<ClientServiceModel> CreateAsync(CreateClientServiceModel serviceModel);
        Task<PagedResults<IEnumerable<ClientServiceModel>>> GetByIdsAsync(GetClientsByIdsServiceModel model);
    }
}
