using Feature.Client;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Services.ApiResponseServices;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Clients.ApiRequestModels;
using Tenant.Portal.Areas.Clients.ApiResponseModels;
using Tenant.Portal.Shared.Configurations;

namespace Tenant.Portal.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientApiController : BaseApiController
    {
        private readonly IApiClientService apiClientService;
        private readonly IApiResponseService apiResponseService;
        private readonly IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly ILogger<ClientApiController> logger;

        public ClientApiController(
            IApiClientService apiClientService,
            IApiResponseService apiResponseService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            ILogger<ClientApiController> logger)
        {
            this.apiClientService = apiClientService;
            this.apiResponseService = apiResponseService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration;
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientRequestModel clientRequestModel)
        {
            try
            {
                var apiRequest = new ApiRequest<ClientRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(clientRequestModel),
                    AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    EndpointAddress = this.servicesEndpointsConfiguration.CurrentValue.ClientApi.Host + this.servicesEndpointsConfiguration.CurrentValue.ClientApi.Endpoints.Client
                };

                var response = await this.apiClientService.PostAsync<ApiRequest<ClientRequestModel>, ClientRequestModel, ClientResponseModel>(apiRequest);

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    response.Message = this.clientLocalizer["ClientAlreadyExists"];
                }
                else
                {
                    response = this.apiResponseService.HandleResponse(response, this.clientLocalizer["ClientSavedSuccessfully"]);
                }

                return this.StatusCode((int)response.StatusCode, response);
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());

                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");

                return this.StatusCode((int)HttpStatusCode.BadRequest, this.apiResponseService.GenerateErrorApiResponse(error));
            }
        }
    }
}
