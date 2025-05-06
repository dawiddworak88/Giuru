using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
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
        private readonly IAsyncComponentModelBuilder<PriceComponentModel, ProductPageViewModel> productPageModelBuilder;

        public ProductController(IAsyncComponentModelBuilder<PriceComponentModel, ProductPageViewModel> productPageModelBuilder)
        {
            this.productPageModelBuilder = productPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            var componentModel = new PriceComponentModel
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName]),
                ContentPageKey = "productPage",
                CurrencyCode = this.User.FindFirst("Currency")?.Value ?? "EUR"
            };

            var viewModel = await this.productPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
