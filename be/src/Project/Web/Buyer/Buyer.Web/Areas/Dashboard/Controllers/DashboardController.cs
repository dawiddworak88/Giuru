using Buyer.Web.Areas.Dashboard.ViewModels;
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

namespace Buyer.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel> dashboardPageModelBuilder;

        public DashboardController(IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel> dashboardPageModelBuilder)
        {
            this.dashboardPageModelBuilder = dashboardPageModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                ContentPageKey = "dashboardPage",
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.dashboardPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
