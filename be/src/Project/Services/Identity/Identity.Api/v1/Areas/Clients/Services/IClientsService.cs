using Foundation.GenericRepository.Paginations;
using Identity.Api.v1.Areas.Clients.Models;
using Identity.Api.v1.Areas.Clients.ResultModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.v1.Areas.Clients.Services
{
    public interface IClientsService
    {
        Task<PagedResults<IEnumerable<ClientResultModel>>> GetAsync(GetClientsModel model);
        Task<ClientResultModel> GetAsync(GetClientModel model);
        Task DeleteAsync(DeleteClientModel model);
        Task<ClientResultModel> UpdateAsync(UpdateClientModel serviceModel);
        Task<ClientResultModel> CreateAsync(CreateClientModel serviceModel);
    }
}
