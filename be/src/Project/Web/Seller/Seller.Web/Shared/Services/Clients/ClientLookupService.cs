using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Repositories.Clients;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Clients
{
    public class ClientLookupService : IClientLookupService
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IOptions<AppSettings> _options;

        // A request prices for one client, so a single slot is enough; keeping the task rather
        // than the client means concurrent callers share the in-flight fetch.
        private Guid? _clientId;
        private Task<Client> _client;

        public ClientLookupService(
            IClientsRepository clientsRepository,
            IOptions<AppSettings> options)
        {
            _clientsRepository = clientsRepository;
            _options = options;
        }

        public Task<Client> GetAsync(string token, Guid? clientId)
        {
            if (_client is not null && _clientId == clientId)
            {
                return _client;
            }

            _clientId = clientId;
            _client = _clientsRepository.GetClientAsync(token, _options.Value.DefaultCulture, clientId);

            return _client;
        }
    }
}
