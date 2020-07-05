using Feature.Product;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Services.ApiResponseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Orders.ApiRequestModels;
using Tenant.Portal.Shared.Configurations;

namespace Tenant.Portal.Areas.Clients.ApiControllers
{
    [Area("Orders")]
    public class ImportOrderApiController : BaseApiController
    {
        private readonly IApiClientService apiClientService;
        private readonly IApiResponseService apiResponseService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly IStringLocalizer productLocalizer;
        private readonly ILogger<ImportOrderApiController> logger;

        public ImportOrderApiController(
            IApiClientService apiClientService,
            IApiResponseService apiResponseService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            IStringLocalizer<ProductResources> productLocalizer,
            ILogger<ImportOrderApiController> logger)
        {
            this.apiClientService = apiClientService;
            this.apiResponseService = apiResponseService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.productLocalizer = productLocalizer;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Validate(ImportOrderRequestModel requestModel)
        {
            try
            {
                return this.Ok();
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
