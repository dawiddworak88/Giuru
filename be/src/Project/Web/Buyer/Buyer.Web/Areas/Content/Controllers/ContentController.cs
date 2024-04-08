using Buyer.Web.Areas.Content.ComponentModels;
using Buyer.Web.Areas.Content.ViewModel;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.Controllers
{
    [Area("Content")]
    public class ContentController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<SlugContentComponentModel, SlugPageViewModel> _slugPageModelBuilder;

        public ContentController(
            IAsyncComponentModelBuilder<SlugContentComponentModel, SlugPageViewModel> slugPageModelBuilder)
        {
            _slugPageModelBuilder = slugPageModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            var componentModel = new SlugContentComponentModel
            {
                Slug = slug,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                Name = User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await _slugPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
