using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.TeamMembers.ViewModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.Controllers
{
    [Area("TeamMembers")]
    public class TeamMemberController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberPageViewModel> teamMemberPageModelBuilder;

        public TeamMemberController(
            IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberPageViewModel> teamMemberPageModelBuilder)
        {
            this.teamMemberPageModelBuilder = teamMemberPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.teamMemberPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
