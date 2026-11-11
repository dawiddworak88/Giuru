using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Repositories.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Clients
{
    public class ClientLookupService : IClientLookupService
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IOptions<AppSettings> _options;

        // A request prices for one client, so a single slot is enough. The slot records the token
        // it was filled with as well as the client id, because a client read under one token must
        // never be handed to a caller presenting another - callers happen to share one token per
        // scope today, but nothing in the signature says they must. Holding the task rather than
        // the awaited client is what lets concurrent callers share the in-flight fetch, and the
        // gate is what makes that true: without it two callers that both miss would both fetch.
        private readonly Lock _gate = new();
        private CachedClient _cached;

        public ClientLookupService(
            IClientsRepository clientsRepository,
            IOptions<AppSettings> options)
        {
            _clientsRepository = clientsRepository;
            _options = options;
        }

        public Task<Client> GetAsync(string token, Guid? clientId)
        {
            lock (_gate)
            {
                if (_cached is not null
                    && _cached.ClientId == clientId
                    && string.Equals(_cached.Token, token, StringComparison.Ordinal))
                {
                    // A read that failed is not a result worth sharing: handing the same faulted
                    // task to every later consumer would turn one transient repository failure into
                    // a failure of the whole request. Drop it and let this caller re-read instead.
                    // The caller that originally awaited it still observes its own exception.
                    if (!_cached.Client.IsFaulted && !_cached.Client.IsCanceled)
                    {
                        return _cached.Client;
                    }

                    _cached = null;
                }

                // The repository call is async, so only its synchronous prefix runs under the
                // gate; the task is stored here and awaited by the caller outside it.
                var client = _clientsRepository.GetClientAsync(token, _options.Value.DefaultCulture, clientId);

                _cached = new CachedClient(token, clientId, client);

                return client;
            }
        }

        private sealed record CachedClient(string Token, Guid? ClientId, Task<Client> Client);
    }
}
