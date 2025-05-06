using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
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
    public class AvailableProductsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<PriceComponentModel, AvailableProductsPageViewModel> availableProductsPageModelBuilder;

        public AvailableProductsController(IAsyncComponentModelBuilder<PriceComponentModel, AvailableProductsPageViewModel> availableProductsPageModelBuilder)
        {
            this.availableProductsPageModelBuilder = availableProductsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new PriceComponentModel
            { 
                ContentPageKey = "availableProductsPage",
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName]),
                CurrencyCode = this.User.FindFirst("Currency")?.Value ?? "EUR"
            };

            var viewModel = await this.availableProductsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
