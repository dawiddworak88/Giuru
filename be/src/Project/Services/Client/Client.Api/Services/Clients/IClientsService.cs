using Client.Api.ServicesModels.Clients;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Clients
{
    public interface IClientsService
    {
        PagedResults<IEnumerable<ClientServiceModel>> Get(GetClientsServiceModel model);
        Task<ClientServiceModel> GetAsync(GetClientServiceModel model);
        Task<ClientServiceModel> GetByEmailAsync(GetClientByEmailServiceModel model);
        Task<ClientServiceModel> GetByOrganisationAsync(GetClientByOrganisationServiceModel model);
        Task DeleteAsync(DeleteClientServiceModel model);
        Task<ClientServiceModel> UpdateAsync(UpdateClientServiceModel serviceModel);
        Task<ClientServiceModel> CreateAsync(CreateClientServiceModel serviceModel);
        PagedResults<IEnumerable<ClientServiceModel>> GetByIds(GetClientsByIdsServiceModel model);
    }
}
