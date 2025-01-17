using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Approvals;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{

    [Area("Clients")]
    public class ApprovalsApiController : BaseApiController
    {
        private readonly IApprovalsRepository _approvalsRepository;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public ApprovalsApiController(
            IApprovalsRepository approvalsRepository,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _approvalsRepository = approvalsRepository;
            _clientLocalizer = clientLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var clientApprovals = await _approvalsRepository.GetAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentCulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Approval.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, clientApprovals);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ClientRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentCulture.Name;

            await _approvalsRepository.SaveAsync(token, language, model.Id, model.Name);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("ClientApprovalSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentCulture.Name;

            await _approvalsRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("ClientApprovalDeletedSuccessfully").Value });
        }
    }
}
