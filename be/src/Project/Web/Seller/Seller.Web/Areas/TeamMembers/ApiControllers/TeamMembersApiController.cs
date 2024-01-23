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
        private readonly ITeamMembersRepository _teamMembersRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;

        public TeamMembersApiController(
            ITeamMembersRepository teamMembersRepository,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            IOptions<AppSettings> options)
        {
            _teamMembersRepository = teamMembersRepository;
            _options = options;
            _teamMembersLocalizer = teamMembersLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var teamMembers = await _teamMembersRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(TeamMember.Email)} desc");

            return StatusCode((int)HttpStatusCode.OK, teamMembers);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TeamMemberRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var teamMemberId = await _teamMembersRepository.SaveAsync(token, language, model.Id, model.FirstName, model.LastName, model.Email, model.IsActive, _options.Value.SellerUrl);

            return StatusCode((int)HttpStatusCode.OK, new { Id = teamMemberId, Message = _teamMembersLocalizer.GetString("SuccessfullySavedTeamMember").Value });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _teamMembersRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _teamMembersLocalizer.GetString("SuccessfullyDeletedTeamMember").Value });
        }
    }
}
