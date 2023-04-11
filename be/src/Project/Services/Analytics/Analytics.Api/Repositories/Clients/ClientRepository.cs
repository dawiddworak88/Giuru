using Analytics.Api.Configurations;
using Analytics.Api.DomainModels;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Analytics.Api.Repositories.Clients
{
    public class ClientRepository : IClientRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public ClientRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task<Client> GetAsync(string token, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.ClientsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Client>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
