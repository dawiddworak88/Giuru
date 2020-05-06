using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Services.ApiResponseServices;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.ApiRequestModels;
using Tenant.Portal.Areas.Products.ApiResponseModels;
using Tenant.Portal.Areas.Products.ViewModels;
using Tenant.Portal.Shared.Configurations;

namespace Tenant.Portal.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductDetailController : BaseController
    {
        private readonly IApiClientService apiClientService;
        private readonly IApiResponseService apiResponseService;
        private readonly IModelBuilder<ProductDetailPageViewModel> productDetailPageModelBuilder;
        private readonly IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration;
        private readonly ILogger<ProductDetailController> logger;

        public ProductDetailController(
            IApiClientService apiClientService,
            IApiResponseService apiResponseService,
            IModelBuilder<ProductDetailPageViewModel> productDetailPageModelBuilder,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            ILogger<ProductDetailController> logger)
        {
            this.apiClientService = apiClientService;
            this.apiResponseService = apiResponseService;
            this.productDetailPageModelBuilder = productDetailPageModelBuilder;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var apiRequest = new ApiRequest<ProductRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(new ProductRequestModel()),
                    AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    EndpointAddress = this.servicesEndpointsConfiguration.CurrentValue.Api.Host + this.servicesEndpointsConfiguration.CurrentValue.Api.Endpoints.Product
                };

                var response = await this.apiClientService.PostAsync<ApiRequest<ProductRequestModel>, ProductRequestModel, ProductResponseModel>(apiRequest);

                if (response.IsSuccessStatusCode)
                {
                    return this.RedirectToAction(nameof(Index), new { id = response.Data.Id });
                }

                return this.StatusCode((int)response.StatusCode);
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());

                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");

                return this.StatusCode((int)HttpStatusCode.BadRequest, this.apiResponseService.GenerateErrorApiResponse(error));
            }
        }

        public IActionResult Index(Guid id)
        {
            var viewModel = this.productDetailPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
