using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Services.ApiResponseServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
    public class ProductsApiController : BaseApiController
    {
        private readonly IApiClientService apiClientService;
        private readonly IApiResponseService apiResponseService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly ILogger<ClientApiController> logger;

        public ProductsApiController(
            IApiClientService apiClientService,
            IApiResponseService apiResponseService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            ILogger<ClientApiController> logger)
        {
            this.apiClientService = apiClientService;
            this.apiResponseService = apiResponseService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            try
            {
                var productsRequestModel = new ProductsRequestModel
                {
                    Language = CultureInfo.CurrentUICulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage
                };

                var apiRequest = new ApiRequest<ProductsRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(productsRequestModel),
                    AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Products
                };

                var response = await this.apiClientService.GetAsync<ApiRequest<ProductsRequestModel>, ProductsRequestModel, ProductsResponseModel>(apiRequest);

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
