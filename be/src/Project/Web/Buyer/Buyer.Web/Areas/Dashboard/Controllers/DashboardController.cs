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
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel> _dashboardPageModelBuilder;

        public DashboardController(
            IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel> dashboardPageModelBuilder)
        {
            _dashboardPageModelBuilder = dashboardPageModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                ContentPageKey = "dashboardPage",
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await _dashboardPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
