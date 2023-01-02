using Buyer.Web.Areas.Products.ViewModels;
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
    public class OutletController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel> outletPageModelBuilder;

        public OutletController(
            IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel> outletPageModelBuilder)
        {
            this.outletPageModelBuilder = outletPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                ContentPageKey = "outletPage",
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.outletPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
