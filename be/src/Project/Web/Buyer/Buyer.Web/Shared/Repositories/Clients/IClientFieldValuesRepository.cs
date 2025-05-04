using Buyer.Web.Shared.DomainModels.Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public interface IClientFieldValuesRepository
    {
        Task<IEnumerable<ClientFieldValue>> GetAsync(string token, string language, Guid? clientId);
    }
}
