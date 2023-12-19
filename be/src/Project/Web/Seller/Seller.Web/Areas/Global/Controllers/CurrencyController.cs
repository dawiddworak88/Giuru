using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Global.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.Controllers
{
    [Area("Global")]
    public class CurrencyController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CurrencyPageViewModel> _currencyPageModelBuilder;

        public CurrencyController(IAsyncComponentModelBuilder<ComponentModelBase, CurrencyPageViewModel> currencyPageModelBuilder)
        {
            _currencyPageModelBuilder = currencyPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await _currencyPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
