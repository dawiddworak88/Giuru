using Feature.Client.Models;
using Feature.Client.ResultModels;
using System.Threading.Tasks;

namespace Feature.Client.Services
{
    public interface IClientService
    {
        Task<ClientsResultModel> GetAsync(GetClientsModel getClientsModel);
        Task<CreateClientResultModel> CreateAsync(CreateClientModel model);
    }
}
