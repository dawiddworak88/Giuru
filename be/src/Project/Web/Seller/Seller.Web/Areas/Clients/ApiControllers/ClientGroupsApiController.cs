using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientGroupsApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientGroupsRepository clientGroupsRepository;

        public ClientGroupsApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientGroupsRepository clientGroupsRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.clientGroupsRepository = clientGroupsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] GroupRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groupId = await this.clientGroupsRepository.SaveAsync(token, language, model.Id, model.Name);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = groupId, Message = this.clientLocalizer.GetString("GroupSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientGroupsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("GroupDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groups = await this.clientGroupsRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientGroup.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, groups);
        }
    }
}
