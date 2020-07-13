using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Portal.Areas.Products.ViewModels;
using Seller.Portal.Shared.ComponentModels;

namespace Seller.Portal.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductDetailController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailPageViewModel> productDetailPageModelBuilder;

        public ProductDetailController(IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailPageViewModel> productDetailPageModelBuilder)
        {
            this.productDetailPageModelBuilder = productDetailPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Id = id
            };

            var viewModel = await this.productDetailPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
