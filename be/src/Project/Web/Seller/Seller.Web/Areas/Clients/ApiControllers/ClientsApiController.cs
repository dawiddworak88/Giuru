using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Helpers;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientsApiController : BaseApiController
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IStringLocalizer clientLocalizer;

        public ClientsApiController(
            IClientsRepository clientsRepository,
            IStringLocalizer<ProductResources> clientLocalizer)
        {
            this.clientsRepository = clientsRepository;
            this.clientLocalizer = clientLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var clients = await this.clientsRepository.GetClientsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value),
                pageIndex,
                itemsPerPage);

            return this.StatusCode((int)HttpStatusCode.OK, clients);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveClientRequestModel model)
        {
            var categoryId = await this.clientsRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Name,
                model.Email,
                model.CommunicationLanguage);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = categoryId, Message = this.clientLocalizer.GetString("ClientSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await this.clientsRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("ClientDeletedSuccessfully").Value });
        }
    }
}
