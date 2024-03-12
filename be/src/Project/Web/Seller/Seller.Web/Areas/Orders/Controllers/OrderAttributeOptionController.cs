using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Areas.Orders.ComponentModels;
using Microsoft.AspNetCore.Authentication;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrderAttributeOptionController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionPageViewModel> _orderAttributeOptionPageViewModel;

        public OrderAttributeOptionController(
            IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionPageViewModel> orderAttributeOptionPageViewModel)
        {
            _orderAttributeOptionPageViewModel = orderAttributeOptionPageViewModel;
        }

        public async Task<IActionResult> New(Guid? id)
        {
            var componentModel = new OrderAttributeOptionComponentModel
            {
                OrderAttributeOptionId = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
            };

            var viewModel = await _orderAttributeOptionPageViewModel.BuildModelAsync(componentModel);

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new OrderAttributeOptionComponentModel
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
            };

            var viewModel = await _orderAttributeOptionPageViewModel.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
