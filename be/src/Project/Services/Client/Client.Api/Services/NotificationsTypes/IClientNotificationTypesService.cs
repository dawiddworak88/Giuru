using Client.Api.ServicesModels.Notification;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.NotificationsType
{
    public interface IClientNotificationTypesService
    {
        PagedResults<IEnumerable<ClientNotificationTypeServiceModel>> Get(GetClientNotificationTypesServiceModel model);
        Task<ClientNotificationTypeServiceModel> GetAsync(GetClientNotificationTypeServiceModel model);
        Task DeleteAsync(DeleteClientNotificationTypeServiceModel model);
        Task<ClientNotificationTypeServiceModel> UpdateAsync(UpdateClientNotificationTypeServiceModel model);
        Task<ClientNotificationTypeServiceModel> CreateAsync(CreateClientNotificationTypeServiceModel model);
        PagedResults<IEnumerable<ClientNotificationTypeServiceModel>> GetByIds(GetClientNotificationTypeByIdsServiceModel model);
    }
}
