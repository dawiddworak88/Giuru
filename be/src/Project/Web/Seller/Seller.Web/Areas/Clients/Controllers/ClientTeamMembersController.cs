using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientTeamMembersController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientTeamMembersPageViewModel> _pageModelBuilder;

        public ClientTeamMembersController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientTeamMembersPageViewModel> pageModelBuilder)
        {
            _pageModelBuilder = pageModelBuilder;
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

            var viewModel = await _pageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
