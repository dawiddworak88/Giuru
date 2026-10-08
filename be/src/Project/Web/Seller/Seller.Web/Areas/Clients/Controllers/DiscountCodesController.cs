using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Filters;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    [RequireGrula]
    [Authorize(Policy = "SellerOnly")]
    public class DiscountCodesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodesPageViewModel> _discountCodesPageModelBuilder;

        public DiscountCodesController(
            IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodesPageViewModel> discountCodesPageModelBuilder)
        {
            _discountCodesPageModelBuilder = discountCodesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
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
