using Seller.Web.Shared.DomainModels.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.Clients
{
    public interface IClientGroupsRepository
    {
        Task<IEnumerable<ClientGroup>> GetAsync(string token, string language);
    }
}
