using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Shared.Definitions;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Models.Response;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Identity.Api.Configurations;
using Foundation.ApiExtensions.Models.Request;

namespace Identity.Api.Areas.Accounts.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public ClientRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<Guid> AddMarketingApprovals(string token, string language, Guid? id, Guid? organisationId, IEnumerable<string> marketingApprovals)
        {
            var requestModel = new SaveClientRequestModel
            {
                Id = id,
                OrganisationId = organisationId,
                MarketingApprovals = marketingApprovals
            };

            var apiRequest = new ApiRequest<SaveClientRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.ClientsApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<SaveClientRequestModel>, SaveClientRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<Guid?> GetClientByOrganistationId(string language, string token, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Identity.ClientByOrganisationApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, RequestModelBase>(apiRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if(response.IsSuccessStatusCode && response.Data != null) 
            {
                return response.Data.Id;
            }

            return default;
        }
    }
}
