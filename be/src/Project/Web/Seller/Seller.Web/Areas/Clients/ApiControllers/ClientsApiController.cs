using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.Claims;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.FieldValues;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Areas.Shared.Repositories.UserApprovals;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Identity;
using Seller.Web.Shared.Repositories.Organisations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientsApiController : BaseApiController
    {
        private readonly IOrganisationsRepository _organisationsRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly IClaimsCacheInvalidatorService _cacheInvalidatorService;

        public ClientsApiController(
            IOrganisationsRepository organisationsRepository,
            IClientsRepository clientsRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientGroupsRepository clientGroupsRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IClaimsCacheInvalidatorService cacheInvalidatorService)
        {
            _organisationsRepository = organisationsRepository;
            _clientsRepository = clientsRepository;
            _clientLocalizer = clientLocalizer;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _cacheInvalidatorService = cacheInvalidatorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var clients = await _clientsRepository.GetClientsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Client.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, clients);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveClientRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            // The organisation is looked up by email, so a new email moves the client to another organisation. Read the
            // one it is in now before the save: its members must stop pricing for the client as well.
            var previousOrganisationId = model.Id.HasValue
                ? (await _clientsRepository.GetClientAsync(token, language, model.Id))?.OrganisationId
                : null;

            var organisation = await _organisationsRepository.GetAsync(token, language, model.Email);

            var organisationId = organisation is not null
                ? organisation.Id
                : await _organisationsRepository.SaveAsync(token, language, model.Name, model.Email, model.CommunicationLanguage);

            var clientId = await _clientsRepository.SaveAsync(token, language, model.Id, model.Name, model.Email, model.CommunicationLanguage, model.CountryId, model.PreferedCurrencyId, model.PhoneNumber, model.IsDisabled, organisationId.Value, model.ClientGroupIds, model.ClientManagerIds, model.DefaultDeliveryAddressId, model.DefaultBillingAddressId);

            // Renamed only once the client is saved: the save refuses an email that is another client's, and the
            // organisation found by it is then that client's, which keeps its name.
            if (organisation is not null && organisation.Name != model.Name)
            {
                await _organisationsRepository.UpdateAsync(token, language, organisation.Id.Value, model.Name);
            }

            await _organisationsRepository.UpdateStatusAsync(token, language, organisationId.Value, model.IsDisabled);

            if (model.FieldsValues is not null && model.FieldsValues.Any())
            {
                await _clientFieldValuesRepository.SaveAsync(token, language, clientId,
                  model.FieldsValues.Select(x => new ApiClientFieldValue
                  {
                      FieldDefinitionId = x.FieldDefinitionId,
                      FieldValue = x.FieldValue
                  }));
            }

            await _cacheInvalidatorService.InvalidateAsync(organisationId);

            if (previousOrganisationId.HasValue && previousOrganisationId != organisationId)
            {
                await _cacheInvalidatorService.InvalidateAsync(previousOrganisationId);
            }

            return StatusCode((int)HttpStatusCode.OK, new { Id = clientId, Message = _clientLocalizer.GetString("ClientSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var client = await _clientsRepository.GetClientAsync(token, language, id);

            await _clientsRepository.DeleteAsync(token, language, id);

            if (client is not null)
            {
                await _organisationsRepository.UpdateStatusAsync(token, language, client.OrganisationId, true);

                // Only once the client is deleted: a buyer request in between would otherwise cache it again.
                await _cacheInvalidatorService.InvalidateAsync(client.OrganisationId);
            }

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("ClientDeletedSuccessfully").Value });
        }
    }
}
