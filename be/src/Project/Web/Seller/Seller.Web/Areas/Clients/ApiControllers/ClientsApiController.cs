using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.Claims;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Foundation.Account.Definitions;
using Seller.Web.Shared.Configurations;
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
        private readonly IOptions<AppSettings> _options;

        public ClientsApiController(
            IOrganisationsRepository organisationsRepository,
            IClientsRepository clientsRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientGroupsRepository clientGroupsRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IClaimsCacheInvalidatorService cacheInvalidatorService,
            IOptions<AppSettings> options)
        {
            _organisationsRepository = organisationsRepository;
            _clientsRepository = clientsRepository;
            _clientLocalizer = clientLocalizer;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _cacheInvalidatorService = cacheInvalidatorService;
            _options = options;
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
            // Without Grula the client form does not send the field, and a crafted request must not change assignments
            // either: null means "leave unchanged". Anything that does change them is for sellers only, and is refused
            // before any organisation or client side effect.
            var discountCodeIds = _options.Value.IsGrulaConfigured ? model.DiscountCodeIds : null;

            if (discountCodeIds is not null && User.IsInRole(AccountConstants.Roles.Seller) is false)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            Guid? organisationId;

            var organisation = await _organisationsRepository.GetAsync(token, language, model.Email);

            if (organisation is not null)
            {
                organisationId = organisation.Id;
            }
            else
            {
                organisationId = await _organisationsRepository.SaveAsync(token, language, model.Name, model.Email, model.CommunicationLanguage);
            }

            var clientId = await _clientsRepository.SaveAsync(token, language, model.Id, model.Name, model.Email, model.CommunicationLanguage, model.CountryId, model.PreferedCurrencyId, model.PhoneNumber, model.IsDisabled, organisationId.Value, model.ClientGroupIds, model.ClientManagerIds, model.DefaultDeliveryAddressId, model.DefaultBillingAddressId, discountCodeIds);

            if (model.FieldsValues is not null && model.FieldsValues.Any())
            {
                await _clientFieldValuesRepository.SaveAsync(token, language, clientId,
                  model.FieldsValues.Select(x => new ApiClientFieldValue
                  {
                      FieldDefinitionId = x.FieldDefinitionId,
                      FieldValue = x.FieldValue
                  }));
            }

            await _cacheInvalidatorService.InvalidateAsync(model.Email);

            return StatusCode((int)HttpStatusCode.OK, new { Id = clientId, Message = _clientLocalizer.GetString("ClientSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientsRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("ClientDeletedSuccessfully").Value });
        }
    }
}
