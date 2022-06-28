using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Roles;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientRolesApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientRolesRepository clientRolesRepository;

        public ClientRolesApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientRolesRepository clientRolesRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.clientRolesRepository = clientRolesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] RoleRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var roleId = await this.clientRolesRepository.SaveAsync(token, language, model.Id, model.Name);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = roleId, Message = this.clientLocalizer.GetString("RoleSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientRolesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("RoleDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var roles = await this.clientRolesRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientRole.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, roles);
        }
    }
}