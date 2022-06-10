using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Managers;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientAccountManagersApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientAccountManagersRepository clientManagersRepository;

        public ClientAccountManagersApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientAccountManagersRepository clientManagersRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.clientManagersRepository = clientManagersRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] AccountManagerRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var managerId = await this.clientManagersRepository.SaveAsync(token, language, model.Id, model.FirstName, model.LastName, model.Email, model.PhoneNumber);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = managerId, Message = this.clientLocalizer.GetString("ManagerSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientManagersRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("ManagerDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var managers = await this.clientManagersRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientAccountManager.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, managers);
        }
    }
}
