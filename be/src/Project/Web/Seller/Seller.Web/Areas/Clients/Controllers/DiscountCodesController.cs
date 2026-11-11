using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Configurations;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    // Discount Codes exist only while Grula is configured, so the page answers 404 without it.
    [Area("Clients")]
    public class DiscountCodesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodesPageViewModel> _discountCodesPageModelBuilder;
        private readonly IOptions<AppSettings> _options;

        public DiscountCodesController(
            IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodesPageViewModel> discountCodesPageModelBuilder,
            IOptions<AppSettings> options)
        {
            _discountCodesPageModelBuilder = discountCodesPageModelBuilder;
            _options = options;
        }

        public async Task<IActionResult> Index()
        {
            if (!_options.Value.IsGrulaConfigured)
            {
                return NotFound();
            }

            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _discountCodesPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
