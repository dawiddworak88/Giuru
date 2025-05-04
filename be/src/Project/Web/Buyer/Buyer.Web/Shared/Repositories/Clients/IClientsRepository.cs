using Buyer.Web.Shared.DomainModels.Clients;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public interface IClientsRepository
    {
        Task<Client> GetClientAsync(string token, string language);
        Task<Client> GetClientByEmailAsync(string token, string language, string email);
    }
}
