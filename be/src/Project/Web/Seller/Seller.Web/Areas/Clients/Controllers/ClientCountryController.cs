using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Foundation.Account.Definitions;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientCountryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientCountryPageViewModel> clientCountryPageModelBuilder;

        public ClientCountryController(IAsyncComponentModelBuilder<ComponentModelBase, ClientCountryPageViewModel> clientCountryPageModelBuilder)
        {
            this.clientCountryPageModelBuilder = clientCountryPageModelBuilder;
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
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.clientCountryPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
