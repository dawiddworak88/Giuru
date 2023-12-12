using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Shared.Definitions;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using System;
using System.Threading.Tasks;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Identity.Api.Configurations;
using Foundation.ApiExtensions.Models.Request;
using Identity.Api.Areas.Accounts.Models;
using Foundation.ApiExtensions.Models.Response;

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

        public async Task<Guid?> SaveMarketingApprovals(string language, string token, Client client)
        {
            var requestModel = new SaveClientRequestModel
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                CommunicationLanguage = client.CommunicationLanguage,
                PhoneNumber = client.PhoneNumber,
                CountryId = client.CountryId,
                OrganisationId = client.OrganisationId,
                ClientGroupIds = client.ClientGroupIds,
                ClientManagerIds = client.ClientManagerIds,
                DefaultDeliveryAddressId = client.DefaultDeliveryAddressId,
                LastModifiedDate = client.LastModifiedDate,
                CreatedDate = client.CreatedDate,
                MarketingApprovals = client.MarketingApprovals
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
                return response.Data.Id;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<Client> GetClientByOrganistationId(string language, string token, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Identity.ClientByOrganisationApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Client>(apiRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null) 
            {
                return response.Data;
            }

            return default;
        }
    }
}
