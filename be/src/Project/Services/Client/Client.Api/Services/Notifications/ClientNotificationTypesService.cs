using Client.Api.ServicesModels.Notification;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Notifications
{
    public class ClientNotificationTypesService : IClientNotificationTypesService
    {
        public async Task<ClientNotificationTypeServiceModel> CreateAsync(CreateClientNotificationTypeServiceModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task Delete(DeleteClientNotificationTypeServiceModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedResults<IEnumerable<ClientNotificationTypeServiceModel>>> GetAsync(GetClientNotificationTypesServiceModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ClientNotificationTypeServiceModel> GetAsync(GetClientNotificationTypeServiceModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ClientNotificationTypeServiceModel> UpdateAsync(UpdateClientNotificationTypeServiceModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
