using Foundation.ApiExtensions.Services;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
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
            return this.BadRequest();
        }

        public IActionResult Index(Guid id)
        {
            var viewModel = this.productDetailPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
