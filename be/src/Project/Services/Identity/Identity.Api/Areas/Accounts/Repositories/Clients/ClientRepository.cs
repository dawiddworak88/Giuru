using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Shared.Definitions;
using System;
using System.Threading.Tasks;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Identity.Api.Configurations;
using Foundation.ApiExtensions.Models.Request;
using Identity.Api.Areas.Accounts.Models;

namespace Identity.Api.Areas.Accounts.Repositories.Clients
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

        public async Task<Client> GetByOrganisationAsync(string token, string language, Guid? id)
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
