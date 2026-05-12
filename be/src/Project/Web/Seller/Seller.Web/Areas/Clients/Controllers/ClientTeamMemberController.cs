using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
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
    public class ClientTeamMemberController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientTeamMemberPageViewModel> _pageModelBuilder;

        public ClientTeamMemberController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientTeamMemberPageViewModel> pageModelBuilder)
        {
            _pageModelBuilder = pageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "ClientTeamMembers", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            }

            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _pageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
