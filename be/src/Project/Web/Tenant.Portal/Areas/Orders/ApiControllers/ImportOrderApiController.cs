using Feature.ImportOrder.Services;
using Feature.Order;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Services.ApiResponseServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Orders.ApiRequestModels;
using Tenant.Portal.Areas.Orders.ApiResponseModels;
using Tenant.Portal.Shared.Configurations;

namespace Tenant.Portal.Areas.Clients.ApiControllers
{
    [Area("Orders")]
    public class ImportOrderApiController : BaseApiController
    {
        private readonly IImportOrderServiceFactory importOrderServiceFactory;
        private readonly IApiClientService apiClientService;
        private readonly IApiResponseService apiResponseService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly IStringLocalizer orderLocalizer;
        private readonly ILogger<ImportOrderApiController> logger;

        public ImportOrderApiController(
            IImportOrderServiceFactory importOrderServiceFactory,
            IApiClientService apiClientService,
            IApiResponseService apiResponseService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            IStringLocalizer<OrderResources> orderLocalizer,
            ILogger<ImportOrderApiController> logger)
        {
            this.importOrderServiceFactory = importOrderServiceFactory;
            this.apiClientService = apiClientService;
            this.apiResponseService = apiResponseService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.orderLocalizer = orderLocalizer;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Validate(ImportOrderRequestModel requestModel)
        {
            try
            {
                if (requestModel.OrderFile != null)
                {
                    var importOrderService = this.importOrderServiceFactory.GetImportOrderService(requestModel.OrderFile.FileName);

                    if (importOrderService != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            requestModel.OrderFile.CopyTo(stream);

                            var order = importOrderService.ImportOrder(stream);

                            var validateOrderRequestModel = new ValidateOrderRequestModel
                            {
                                Language = CultureInfo.CurrentUICulture.Name,
                                ClientId = requestModel.ClientId,
                                Order = order
                            };

                            var apiRequest = new ApiRequest<ValidateOrderRequestModel>
                            {
                                Data = this.apiClientService.InitializeRequestModelContext(validateOrderRequestModel),
                                AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                                EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Products
                            };

                            var response = await this.apiClientService.GetAsync<ApiRequest<ValidateOrderRequestModel>, ValidateOrderRequestModel, ValidateOrderResponseModel>(apiRequest);

                            return this.StatusCode((int)response.StatusCode, response);
                        }
                    }
                }

                var validationMessages = new List<string>
                { 
                    this.orderLocalizer.GetString("OrderFileError")
                };

                return this.StatusCode((int)HttpStatusCode.OK, new ValidateOrderResponseModel { ValidationMessages = validationMessages });
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
