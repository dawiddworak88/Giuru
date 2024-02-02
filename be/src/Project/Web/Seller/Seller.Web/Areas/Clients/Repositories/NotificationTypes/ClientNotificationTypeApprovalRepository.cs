using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Seller.Web.Shared.Configurations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Foundation.ApiExtensions.Models.Response;

namespace Seller.Web.Areas.Clients.Repositories.NotificationTypes
{
    public class ClientNotificationTypeApprovalRepository : IClientNotificationTypeApprovalRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public ClientNotificationTypeApprovalRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<IEnumerable<ClientNotificationTypeApproval>> GetAsync(string token, string language, Guid? clientId)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                AccessToken = token,
                Language = language,
                Data = new RequestModelBase(),
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.NotificationTypesApiApprovalEndpoint}/{clientId}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<ClientNotificationTypeApproval>>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task SaveAsync(string token, string language, Guid? clientId, IEnumerable<Guid> notificationTypeIds)
        {
            var requestModel = new ClientNotificationTypeApprovalRequestModel
            {
                ClientId = clientId,
                NotificationTypeIds = notificationTypeIds
            };

            var apiRequest = new ApiRequest<ClientNotificationTypeApprovalRequestModel>
            {
                AccessToken = token,
                Language = language,
                Data = requestModel,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.NotificationTypesApiApprovalEndpoint}}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<ClientNotificationTypeApprovalRequestModel>, ClientNotificationTypeApprovalRequestModel, BaseResponseModel>(apiRequest);

            if(!response.IsSuccessStatusCode) 
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }
    }
}
