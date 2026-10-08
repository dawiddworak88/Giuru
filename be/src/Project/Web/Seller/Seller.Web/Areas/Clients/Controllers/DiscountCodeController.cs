using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Filters;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    [RequireGrula]
    [Authorize(Policy = "SellerOnly")]
    public class DiscountCodeController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodePageViewModel> _discountCodePageModelBuilder;

        public DiscountCodeController(
            IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodePageViewModel> discountCodePageModelBuilder)
        {
            _discountCodePageModelBuilder = discountCodePageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _discountCodePageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
