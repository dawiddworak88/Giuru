using System.Collections.Generic;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Clients.DomainModels;

namespace Tenant.Portal.Areas.Clients.Repositories
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language);
    }
}
