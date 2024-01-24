using Client.Api.ServicesModels.Notification;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Notifications
{
    public interface IClientNotificationTypesService
    {
        Task<PagedResults<IEnumerable<ClientNotificationTypeServiceModel>>> GetAsync(GetClientNotificationTypesServiceModel model);
        Task<ClientNotificationTypeServiceModel> GetAsync(GetClientNotificationTypeServiceModel model);
        Task Delete(DeleteClientNotificationTypeServiceModel model);
        Task<ClientNotificationTypeServiceModel> UpdateAsync(UpdateClientNotificationTypeServiceModel model);
        Task<ClientNotificationTypeServiceModel> CreateAsync(CreateClientNotificationTypeServiceModel model);
    }
}
