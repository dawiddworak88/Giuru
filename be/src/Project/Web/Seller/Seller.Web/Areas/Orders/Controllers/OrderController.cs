using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Orders.ComponetModels;
using Seller.Web.Areas.Orders.ViewModel;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Seller.Web.Areas.Orders.ComponetModels;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrderController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> _orderPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderPageViewModel> _editOrderPageModelBuilder;

        public OrderController(
            IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> orderPageModelBuilder,
            IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderPageViewModel> editOrderPageModelBuilder)
        {
            _orderPageModelBuilder = orderPageModelBuilder;
            _editOrderPageModelBuilder = editOrderPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await _orderPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(Guid? id, string searchTerm)
        {
            var componentModel = new OrdersPageComponentModel
            {
                Id = id,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                SearchTerm = searchTerm
            };

            var viewModel = await _editOrderPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}