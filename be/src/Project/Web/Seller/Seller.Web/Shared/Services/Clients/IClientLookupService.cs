using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Clients
{
    /// <summary>
    /// Request-scoped read of the client a seller is acting for. Registered scoped and memoised,
    /// so the several consumers that need the same client in one request - the price-client
    /// resolver and the callers that also need the client's organisation - share a single fetch
    /// instead of each issuing their own.
    /// </summary>
    public interface IClientLookupService
    {
        Task<Client> GetAsync(string token, Guid? clientId);
    }
}
