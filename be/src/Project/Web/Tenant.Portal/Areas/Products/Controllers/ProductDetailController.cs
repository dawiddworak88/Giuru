using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.ComponentModels;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductDetailController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ProductDetailComponentModel, ProductDetailPageViewModel> productDetailPageModelBuilder;

        public ProductDetailController(IAsyncComponentModelBuilder<ProductDetailComponentModel, ProductDetailPageViewModel> productDetailPageModelBuilder)
        {
            this.productDetailPageModelBuilder = productDetailPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            var componentModel = new ProductDetailComponentModel
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Id = id
            };

            var viewModel = await this.productDetailPageModelBuilder.BuildModelAsync(componentModel);

            return this.View("Index", viewModel);
        }
    }
}
