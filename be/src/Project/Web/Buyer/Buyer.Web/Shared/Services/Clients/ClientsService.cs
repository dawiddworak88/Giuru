using Buyer.Web.Shared.Definitions.CompletionDate;
using Buyer.Web.Shared.Repositories.Clients;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Clients
{
    public class ClientsService : IClientsService
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsService(IClientsRepository clientsRepository)
        { 
            _clientsRepository = clientsRepository;
        }

        public async Task<bool> IsEltapTransportEnableAsync(string token, string language, Guid? sellerId)
        {
            var client = await _clientsRepository.GetClientAsync(token, language, sellerId);

            if (client is not null)
            {
                var clientFieldValues = await _clientsRepository.GetClientFieldValuesAsync(token, language, client.Id);

                if (clientFieldValues.Any(x => x.FieldDefinitionId == ClientFieldsConstants.Transport.Id))
                {
                    var clientField = clientFieldValues.FirstOrDefault(y => y.FieldDefinitionId == ClientFieldsConstants.Transport.Id);

                    if (Guid.Parse(clientField.FieldValue) == ClientFieldsConstants.Transport.EltapTransportId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
