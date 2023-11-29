using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Buyer.Web.Shared.DomainModels.Clients;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public interface IClientAddressesRepository
    {
        Task<ClientAddress> GetAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<ClientAddress>>> GetAsync(string token, string language, Guid? clientId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<ClientAddress>> GetAsync(string token, string language, IEnumerable<Guid> clientAddressesIds);
    }
}
