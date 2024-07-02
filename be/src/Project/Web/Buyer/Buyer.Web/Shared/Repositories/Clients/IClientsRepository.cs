using Buyer.Web.Shared.DomainModels.Clients;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public interface IClientsRepository
    {
        Task<Client> GetClientAsync(string token, string language, Guid? id);
        Task<List<ClientFieldValue>> GetClientFieldValuesAsync(string token, string language, Guid? id);
    }
}
