using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Foundation.Account.Definitions;
using Seller.Web.Areas.Global.ViewModels;

namespace Seller.Web.Areas.Global.Controllers
{
    [Area("Global")]
    public class CountryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CountryPageViewModel> countryPageModelBuilder;

        public CountryController(IAsyncComponentModelBuilder<ComponentModelBase, CountryPageViewModel> countryPageModelBuilder)
        {
            this.countryPageModelBuilder = countryPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = this.User.Identity.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.countryPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
