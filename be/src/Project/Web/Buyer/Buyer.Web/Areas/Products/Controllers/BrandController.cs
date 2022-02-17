using Buyer.Web.Areas.Products.ViewModels.Brands;
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

namespace Buyer.Web.Areas.Brands.Controllers
{
    [Area("Products")]
    public class BrandController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BrandPageViewModel> brandPageModelBuilder;

        public BrandController(IAsyncComponentModelBuilder<ComponentModelBase, BrandPageViewModel> brandPageModelBuilder)
        {
            this.brandPageModelBuilder = brandPageModelBuilder;
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

            var viewModel = await this.brandPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
