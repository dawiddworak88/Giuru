using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.ApiResponseModels;
using Seller.Web.Shared.Configurations;
using Foundation.Localization;

namespace Seller.Web.Areas.Clients.ApiControllers
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
                Data = this.apiClientService.InitializeRequestModelContext(clientRequestModel),
                AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                EndpointAddress = this.servicesEndpointsConfiguration.CurrentValue.Api.Host + this.servicesEndpointsConfiguration.CurrentValue.Api.Endpoints.Client
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<ClientRequestModel>, ClientRequestModel, ClientResponseModel>(apiRequest);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                response.Message = this.clientLocalizer["ClientAlreadyExists"];
            }

            return this.StatusCode((int)response.StatusCode, response);
        }
    }
}
