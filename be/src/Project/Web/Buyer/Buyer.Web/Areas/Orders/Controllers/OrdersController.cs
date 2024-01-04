using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Foundation.Extensions.Helpers;
using System.Linq;
using Foundation.Account.Definitions;
using Buyer.Web.Areas.Orders.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Buyer.Web.Shared.Definitions.Basket;
using System;
using Buyer.Web.Areas.Orders.ComponentModels;

namespace Buyer.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<OrdersPageComponentModel, OrdersPageViewModel> _ordersPageModelBuilder;

        public OrdersController(IAsyncComponentModelBuilder<OrdersPageComponentModel, OrdersPageViewModel> ordersPageModelBuilder)
        {
            _ordersPageModelBuilder = ordersPageModelBuilder;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var componentModel = new OrdersPageComponentModel
            {
                ContentPageKey = "ordersPage",
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(Request.Cookies[BasketConstants.BasketCookieName]),
                SearchTerm = searchTerm
            };

            var viewModel = await _ordersPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
