using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.ViewModels;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientTeamMemberDetailController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ClientTeamMemberComponentModel, ClientTeamMemberDetailPageViewModel> _pageModelBuilder;

        public ClientTeamMemberDetailController(
            IAsyncComponentModelBuilder<ClientTeamMemberComponentModel, ClientTeamMemberDetailPageViewModel> pageModelBuilder)
        {
            _pageModelBuilder = pageModelBuilder;
        }

        public async Task<IActionResult> New(Guid? clientId, Guid? organisationId)
        {
            var componentModel = new ClientTeamMemberComponentModel
            {
                ClientId = clientId,
                OrganisationId = organisationId,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
            };

            var viewModel = await _pageModelBuilder.BuildModelAsync(componentModel);

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Edit(Guid? clientId, Guid? organisationId, Guid? id)
        {
            var componentModel = new ClientTeamMemberComponentModel
            {
                Id = id,
                ClientId = clientId,
                OrganisationId = organisationId,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
            };

            var viewModel = await _pageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
