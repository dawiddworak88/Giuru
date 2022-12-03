using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Identity;
using Seller.Web.Shared.Repositories.Organisations;
using System;
using System.Globalization;
using System.Linq;
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
        private readonly IClientGroupsRepository clientGroupsRepository;

        public ClientsApiController(
            IOrganisationsRepository organisationsRepository,
            IClientsRepository clientsRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IIdentityRepository identityRepository,
            IClientGroupsRepository clientGroupsRepository)
        {
            this.organisationsRepository = organisationsRepository;
            this.clientsRepository = clientsRepository;
            this.clientLocalizer = clientLocalizer;
            this.identityRepository = identityRepository;
            this.clientGroupsRepository = clientGroupsRepository;
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

            var clientId = await this.clientsRepository.SaveAsync(token, language, model.Id, model.Name, model.Email, model.CommunicationLanguage, model.CountryId, model.PhoneNumber, organisationId.Value, model.ClientGroupIds, model.ClientManagerIds);

            if (model.HasAccount)
            {
                await this.identityRepository.UpdateAsync(token, language, clientId, model.Email, model.Name, model.CommunicationLanguage);

                if (model.ClientGroupIds.OrEmptyIfNull().Any())
                {
                    var clientGroups = await this.clientGroupsRepository.GetClientGroupsAsync(token, language, model.ClientGroupIds);

                    if (clientGroups is not null)
                    {
                        await this.identityRepository.AssignRolesAsync(token, language, model.Email, clientGroups.Select(x => x.Name));
                    }
                }
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
