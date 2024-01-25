using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.TeamMembers.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.Controllers
{
    [Area("TeamMembers")]
    public class TeamMembersController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, TeamMembersPageViewModel> _teamMembersPageModelBuilder;

        public TeamMembersController(
            IAsyncComponentModelBuilder<ComponentModelBase, TeamMembersPageViewModel> teamMembersPageModelBuilder)
        {
            _teamMembersPageModelBuilder = teamMembersPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _teamMembersPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
