using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Clients
{
    /// <summary>
    /// Request-scoped read of the client a seller is acting for. Registered scoped and memoised,
    /// so the several consumers that need the same client in one request - the price-client
    /// resolver and the callers that also need the client's organisation - share a single fetch
    /// instead of each issuing their own. The memoised result is keyed on both arguments, so a
    /// caller presenting a different token re-reads rather than inheriting another token's client.
    /// </summary>
    public interface IClientLookupService
    {
        /// <param name="token">Access token for the read. Part of the memoisation key.</param>
        /// <param name="clientId">Client to read, or null for the caller's own. Part of the memoisation key.</param>
        Task<Client> GetAsync(string token, Guid? clientId);
    }
}
