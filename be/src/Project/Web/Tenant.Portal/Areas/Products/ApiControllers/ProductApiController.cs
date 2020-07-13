using Feature.Product;
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
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Seller.Portal.Areas.Products.ApiRequestModels;
using Seller.Portal.Areas.Products.ApiResponseModels;
using Seller.Portal.Shared.Configurations;

namespace Seller.Portal.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductApiController : BaseApiController
    {
        private readonly IApiClientService apiClientService;
        private readonly IApiResponseService apiResponseService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly IStringLocalizer productLocalizer;
        private readonly ILogger<ClientApiController> logger;

        public ProductApiController(
            IApiClientService apiClientService,
            IApiResponseService apiResponseService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            IStringLocalizer<ProductResources> productLocalizer,
            ILogger<ClientApiController> logger)
        {
            this.apiClientService = apiClientService;
            this.apiResponseService = apiResponseService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.productLocalizer = productLocalizer;
            this.logger = logger;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                var deleteProductRequestModel = new DeleteProductRequestModel
                {
                    Language = CultureInfo.CurrentUICulture.Name,
                    Id = id
                };

                var apiRequest = new ApiRequest<DeleteProductRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(deleteProductRequestModel),
                    AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Product
                };

                var response = await this.apiClientService.DeleteAsync<ApiRequest<DeleteProductRequestModel>, DeleteProductRequestModel, DeleteProductResponseModel>(apiRequest);
                
                return this.StatusCode((int)response.StatusCode, this.apiResponseService.EnrichResponseMessage(response, this.productLocalizer["ProductDeletedSuccessfully"]));
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());

                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");

                return this.StatusCode((int)HttpStatusCode.BadRequest, this.apiResponseService.GenerateErrorApiResponse(error));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveProductRequestModel requestModel)
        {
            try
            {
                var productRequestModel = new ProductRequestModel
                { 
                    Id = requestModel.Id,
                    Sku = requestModel.Sku,
                    Name = requestModel.Name,
                    SchemaId = requestModel.SchemaId,
                    FormData = requestModel.FormData.ToString()
                };

                var apiRequest = new ApiRequest<ProductRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(productRequestModel),
                    AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Product
                };

                var response = await this.apiClientService.PostAsync<ApiRequest<ProductRequestModel>, ProductRequestModel, ProductResponseModel>(apiRequest);

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    response.Message = this.productLocalizer["ProductAlreadyExists"];
                }
                else
                {
                    response = this.apiResponseService.EnrichResponseMessage(response, this.productLocalizer["ProductSavedSuccessfully"]);
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
