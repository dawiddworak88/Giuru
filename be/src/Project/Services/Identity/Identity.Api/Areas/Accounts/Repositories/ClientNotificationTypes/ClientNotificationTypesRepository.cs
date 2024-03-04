using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Models;
using System.Threading.Tasks;
using System;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using Identity.Api.Configurations;
using System.Collections.Generic;
using Foundation.ApiExtensions.Models.Request;
using Foundation.GenericRepository.Paginations;
using System.Text.Json;

namespace Identity.Api.Areas.Accounts.Repositories.ClientNotificationTypes
{
    public class ClientNotificationTypesRepository : IClientNotificationTypesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public ClientNotificationTypesRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<IEnumerable<ClientNotificationTypeApproval>> GetByIds(string token, string language, string ids, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var requestModel = new PagedRequestModelBase
            {
                Ids = ids,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.NotificationTypesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientNotificationTypeApproval>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data is not null)
            {
                return response.Data.Data;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<Guid?> SaveAsync(string token, string language, ClientNotificationTypeApprovals model)
        {
            var requestModel = new SaveClientNotificationTypaApprovalRequestModel
            {
                ClientId = model.ClientId,
                NotificationTypeIds = model.ClientApprovals,
            };

            var apiRequest = new ApiRequest<SaveClientNotificationTypaApprovalRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.NotificationTypesApiApprovalEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<SaveClientNotificationTypaApprovalRequestModel>, SaveClientNotificationTypaApprovalRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
