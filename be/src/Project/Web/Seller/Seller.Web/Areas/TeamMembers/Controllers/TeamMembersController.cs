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
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, TeamMembersPageViewModel> teamMembersPageModelBuilder;

        public TeamMembersController(
            IAsyncComponentModelBuilder<ComponentModelBase, TeamMembersPageViewModel> teamMembersPageModelBuilder)
        {
            this.teamMembersPageModelBuilder = teamMembersPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.teamMembersPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
