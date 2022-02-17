using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel> productPageModelBuilder;

        public ProductController(IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel> productPageModelBuilder)
        {
            this.productPageModelBuilder = productPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.productPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
