using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Identity;
using Seller.Web.Shared.Repositories.Organisations;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientsApiController : BaseApiController
    {
        private readonly IOrganisationsRepository organisationsRepository;
        private readonly IClientsRepository clientsRepository;
        private readonly IIdentityRepository identityRepository;
        private readonly IStringLocalizer clientLocalizer;

        public ClientsApiController(
            IOrganisationsRepository organisationsRepository,
            IClientsRepository clientsRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IIdentityRepository identityRepository)
        {
            this.organisationsRepository = organisationsRepository;
            this.clientsRepository = clientsRepository;
            this.clientLocalizer = clientLocalizer;
            this.identityRepository = identityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var clients = await this.clientsRepository.GetClientsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Client.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, clients);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveClientRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            Guid? organisationId;

            var organisation = await this.organisationsRepository.GetAsync(token, language, model.Email);

            if (organisation is not null)
            {
                organisationId = organisation.Id;
            }
            else
            {
                organisationId = await this.organisationsRepository.SaveAsync(token, language, model.Name, model.Email, model.CommunicationLanguage);
            }

            var clientId = await this.clientsRepository.SaveAsync(token, language, model.Id, model.Name, model.Email, model.CommunicationLanguage, model.PhoneNumber, organisationId.Value, model.ClientGroupIds, model.ClientManager);

            if (model.HasAccount)
            {
                await this.identityRepository.UpdateAsync(token, language, clientId, model.Email, model.Name, model.CommunicationLanguage);
            }

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = clientId, Message = this.clientLocalizer.GetString("ClientSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.clientsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.clientLocalizer.GetString("ClientDeletedSuccessfully").Value });
        }
    }
}
