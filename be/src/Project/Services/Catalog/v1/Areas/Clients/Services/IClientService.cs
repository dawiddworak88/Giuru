using Catalog.Api.v1.Areas.Clients.Models;
using Catalog.Api.v1.Areas.Clients.ResultModels;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Clients.Services
{
    public interface IClientService
    {
        Task<ClientsResultModel> GetAsync(GetClientsModel getClientsModel);
        Task<CreateClientResultModel> CreateAsync(CreateClientModel model);
    }
}
