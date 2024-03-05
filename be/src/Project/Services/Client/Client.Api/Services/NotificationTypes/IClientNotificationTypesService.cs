using Client.Api.ServicesModels.NotificationTypes;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.NotificationTypes
{
    public interface IClientNotificationTypesService
    {
        PagedResults<IEnumerable<ClientNotificationTypeServiceModel>> Get(GetClientNotificationTypesServiceModel model);
        Task<ClientNotificationTypeServiceModel> GetAsync(GetClientNotificationTypeServiceModel model);
        Task DeleteAsync(DeleteClientNotificationTypeServiceModel model);
        Task<ClientNotificationTypeServiceModel> UpdateAsync(UpdateClientNotificationTypeServiceModel model);
        Task<ClientNotificationTypeServiceModel> CreateAsync(CreateClientNotificationTypeServiceModel model);
        IEnumerable<ClientNotificationTypeServiceModel> GetByIds(GetClientNotificationTypeByIdsServiceModel model);
    }
}
