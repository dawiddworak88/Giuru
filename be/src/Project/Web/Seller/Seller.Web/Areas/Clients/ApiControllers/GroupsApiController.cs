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
    public class GroupsApiController : BaseApiController
    {
        public IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IGroupsRepository groupsRepository;

        public GroupsApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IGroupsRepository groupsRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.groupsRepository = groupsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] GroupRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groupId = await this.groupsRepository.SaveAsync(token, language, model.Id, model.Name);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = groupId, Message = this.clientLocalizer.GetString("GroupSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.groupsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("GroupDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groups = await this.groupsRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Groups.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, groups);
        }
    }
}
