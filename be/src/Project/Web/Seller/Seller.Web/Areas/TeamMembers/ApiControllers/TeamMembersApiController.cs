using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.TeamMembers.ApiRequestModel;
using Seller.Web.Areas.TeamMembers.DomainModels;
using Seller.Web.Areas.TeamMembers.Repositories;
using Seller.Web.Shared.Configurations;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.ApiControllers
{
    [Area("TeamMembers")]
    public class TeamMembersApiController : BaseApiController
    {
        private readonly ITeamMembersRepository teamMembersRepository;
        private readonly IOptions<AppSettings> options;
        private readonly IStringLocalizer<TeamMembersResources> teamMembersLocalizer;

        public TeamMembersApiController(
            ITeamMembersRepository teamMembersRepository,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            IOptions<AppSettings> options)
        {
            this.teamMembersRepository = teamMembersRepository;
            this.options = options;
            this.teamMembersLocalizer = teamMembersLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var teamMembers = await this.teamMembersRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(TeamMember.Email)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, teamMembers);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TeamMemberRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var teamMemberId = await this.teamMembersRepository.SaveAsync(token, language, model.Id, model.FirstName, model.LastName, model.Email, model.IsActive, this.options.Value.SellerUrl);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = teamMemberId, Message = this.teamMembersLocalizer.GetString("SuccessfullySavedTeamMember").Value });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.teamMembersRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.teamMembersLocalizer.GetString("SuccessfullyDeletedTeamMember").Value });
        }
    }
}
