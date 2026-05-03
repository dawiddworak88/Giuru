using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.ClientTeamMembers;
using Seller.Web.Areas.Shared.Repositories.UserApprovals;
using Seller.Web.Shared.Configurations;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientTeamMembersApiController : BaseApiController
    {
        private readonly IClientTeamMembersRepository _clientTeamMembersRepository;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;
        private readonly IUserApprovalsRepository _userApprovalsRepository;
        private readonly IOptions<AppSettings> _options;


        public ClientTeamMembersApiController(
            IClientTeamMembersRepository clientTeamMembersRepository,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            IUserApprovalsRepository userApprovalsRepository,
            IOptions<AppSettings> options)
        {
            _clientTeamMembersRepository = clientTeamMembersRepository;
            _teamMembersLocalizer = teamMembersLocalizer;
            _userApprovalsRepository = userApprovalsRepository;
            _options = options;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? organisationId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var teamMembers = await _clientTeamMembersRepository.GetAsync(
                token, language, organisationId, searchTerm, pageIndex, itemsPerPage, $"{nameof(TeamMember.Email)} desc");

            return StatusCode((int)HttpStatusCode.OK, teamMembers);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid? organisationId, [FromBody] ClientTeamMemberRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            Guid? teamMemberId;

            try
            {
                teamMemberId = await _clientTeamMembersRepository.SaveAsync(token, language, organisationId, model.Id, model.FirstName, model.LastName, model.Email, model.IsDisabled, _options.Value.BuyerUrl);

                await _userApprovalsRepository.SaveAsync(token, language, model.TeamMemberApprovalIds, teamMemberId.Value);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = _teamMembersLocalizer.GetString("TeamMemberSaveFailed").Value });
            }

            return StatusCode((int)HttpStatusCode.OK, new { Id = teamMemberId, Message = _teamMembersLocalizer.GetString("SuccessfullySavedClientTeamMember").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientTeamMembersRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _teamMembersLocalizer.GetString("SuccessfullyDeletedClientTeamMember").Value });
        }
    }
}
