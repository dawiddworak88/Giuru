using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Clients.DomainModels;

namespace Seller.Web.Areas.Clients.Repositories
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language);
    }
}
