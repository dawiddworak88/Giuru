using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Shared.Configurations;
using Foundation.Localization;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductApiController : BaseApiController
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly IStringLocalizer productLocalizer;

        public ProductApiController(
            IApiClientService apiClientService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.productLocalizer = productLocalizer;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var model = new RequestModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Id = id
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(model),
                AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Product
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);
                
            return this.StatusCode((int)response.StatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveProductRequestModel requestModel)
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

            return this.StatusCode((int)response.StatusCode, response);
        }
    }
}
