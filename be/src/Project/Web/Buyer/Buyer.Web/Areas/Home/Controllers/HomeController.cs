using Buyer.Web.Areas.Home.ViewModel;
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

namespace Buyer.Web.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel> homePageModelBuilder;

        public HomeController(IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel> homePageModelBuilder)
        {
            this.homePageModelBuilder = homePageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.homePageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
