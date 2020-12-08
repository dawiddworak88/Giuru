using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Shared.Configurations;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;

        public ProductsApiController(
            IApiClientService apiClientService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
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
    }
}
