using Buyer.Web.Areas.Orders.ComponentModels;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<OrdersPageComponentModel, StatusOrderPageViewModel> _editOrderPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> _orderPageModelBuilder;

        public OrderController(
            IAsyncComponentModelBuilder<OrdersPageComponentModel, StatusOrderPageViewModel> editOrderPageModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> orderPageModelBuilder)
        {
            _editOrderPageModelBuilder = editOrderPageModelBuilder;
            _orderPageModelBuilder = orderPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                ContentPageKey = "basketPage",
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                Name = User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await _orderPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }

        public async Task<IActionResult> Status(Guid? id, string searchTerm)
        {
            var componentModel = new OrdersPageComponentModel
            {
                Id = id,
                ContentPageKey = "orderPage",
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                Name = User.Identity.Name,
                SearchTerm = searchTerm
            };

            var viewModel = await _editOrderPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
