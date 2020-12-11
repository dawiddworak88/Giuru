using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailPageViewModel> productDetailPageModelBuilder;

        public ProductController(IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailPageViewModel> productDetailPageModelBuilder)
        {
            this.productDetailPageModelBuilder = productDetailPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.productDetailPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
