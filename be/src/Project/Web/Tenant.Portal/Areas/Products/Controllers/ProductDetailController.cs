using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Services;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.ApiRequestModels;
using Tenant.Portal.Areas.Products.ViewModels;
using Tenant.Portal.Shared.Configurations;

namespace Tenant.Portal.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductDetailController : BaseController
    {
        private readonly IApiClientService apiClientService;
        private readonly IModelBuilder<ProductDetailPageViewModel> productDetailPageModelBuilder;
        private readonly IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration;

        public ProductDetailController(
            IApiClientService apiClientService,
            IModelBuilder<ProductDetailPageViewModel> productDetailPageModelBuilder,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration)
        {
            this.apiClientService = apiClientService;
            this.productDetailPageModelBuilder = productDetailPageModelBuilder;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration;
        }

        public async Task<IActionResult> Index()
        {
            var apiRequest = new ApiRequest<ProductRequestModel>
            {
                Data = productRequestModel,
                AccessToken = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                EndpointAddress = this.servicesEndpointsConfiguration.CurrentValue.ClientApi.Host + this.servicesEndpointsConfiguration.CurrentValue.ClientApi.Endpoints.Client
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<ProductRequestModel>, ProductRequestModel, object>(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                return this.RedirectToAction(nameof(Index), new { id = response.Product.Id  });
            }

            return this.BadRequest();
        }

        public IActionResult Index(Guid id)
        {
            var viewModel = this.productDetailPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
