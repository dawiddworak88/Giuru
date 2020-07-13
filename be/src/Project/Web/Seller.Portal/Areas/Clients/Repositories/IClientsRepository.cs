using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Portal.Areas.Clients.DomainModels;

namespace Seller.Portal.Areas.Clients.Repositories
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language);
    }
}
