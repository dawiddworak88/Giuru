using Feature.Client;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Clients.ApiRequestModels;
using Tenant.Portal.Shared.Configurations;

namespace Tenant.Portal.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientApiController : BaseApiController
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public ClientApiController(
            IApiClientService apiClientService, 
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration;
            this.clientLocalizer = clientLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientRequestModel clientRequestModel)
        {
            var apiRequest = new ApiRequest<ClientRequestModel>
            {
                Data = clientRequestModel,
                AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                EndpointAddress = this.servicesEndpointsConfiguration.CurrentValue.ClientApi.Host + this.servicesEndpointsConfiguration.CurrentValue.ClientApi.Endpoints.Client
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<ClientRequestModel>, ClientRequestModel, object>(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                response.Message = this.clientLocalizer["ClientSavedSuccessfully"];
            }

            return this.StatusCode((int)response.StatusCode, response);
        }
    }
}
