using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    // Discount Codes exist only while Grula is configured, so the page answers 404 without it.
    [Area("Clients")]
    public class DiscountCodeController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodePageViewModel> _discountCodePageModelBuilder;
        private readonly IOptions<AppSettings> _options;

        public DiscountCodeController(
            IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodePageViewModel> discountCodePageModelBuilder,
            IOptions<AppSettings> options)
        {
            _discountCodePageModelBuilder = discountCodePageModelBuilder;
            _options = options;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!_options.Value.IsGrulaConfigured)
            {
                return NotFound();
            }

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
